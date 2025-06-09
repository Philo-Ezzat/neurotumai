using NeuroTumAI.Core;
using NeuroTumAI.Core.Dtos.CancerPrediction;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.MriScan;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.ClinicSpecs;
using NeuroTumAI.Core.Specifications.MriScanSpecs;
using NeuroTumAI.Core.Specifications.PatientSpecs;

namespace NeuroTumAI.Service.Services.MriScanService
{
	public class MriScanService : IMriScanService
	{
		private readonly IBlobStorageService _blobStorageService;
		private readonly ICancerDetectionService _cancerDetectionService;
		private readonly IUnitOfWork _unitOfWork;

		public MriScanService(IBlobStorageService blobStorageService, ICancerDetectionService cancerDetectionService, IUnitOfWork unitOfWork)
		{
			_blobStorageService = blobStorageService;
			_cancerDetectionService = cancerDetectionService;
			_unitOfWork = unitOfWork;
		}

		public async Task AutoReviewAsync(int mriId)
		{
			var mriSpecs = new MriScanSpecifications(mriId);

			var mriScanRepository = _unitOfWork.Repository<MriScan>();

			var mriScan = await mriScanRepository.GetWithSpecAsync(mriSpecs);
			mriScan.IsReviewed = true;

			var doctorAssignmentsRepository = _unitOfWork.Repository<DoctorMriAssignment>();

			mriScanRepository.Update(mriScan);
			doctorAssignmentsRepository.RemoveRange(mriScan.DoctorAssignments);

			await _unitOfWork.CompleteAsync();
		}

		public async Task<IReadOnlyList<MriScan>> GetExpiredUnreviewedScansAsync()
		{
			var twelveHoursAgo = DateTime.UtcNow.AddHours(-12);
			var mriScansSpec = new MriScanSpecifications(twelveHoursAgo);

			return await _unitOfWork.Repository<MriScan>().GetAllWithSpecAsync(mriScansSpec);
		}

		public async Task<MriScan> UploadAndProcessMriScanAsync(PredictRequestDto model, string userId)
		{
			using var stream = model.Image.OpenReadStream();
			var fileUrl = await _blobStorageService.UploadFileAsync(stream, model.Image.FileName, "patient-cancer-images");

			var aiResponse = await _cancerDetectionService.PredictCancerAsync(fileUrl);

			var nearbyClinicSpecParams = new ClinicSpecParams
			{
				Latitude = model.Latitude,
				Longitude = model.Longitude,
				PageSize = 15,
				PageIndex = 1
			};

			var clinicSpecs = new NearbyClinicsSpecifications(nearbyClinicSpecParams);
			var nearbyClinics = await _unitOfWork.Repository<Clinic>().GetAllWithSpecAsync(clinicSpecs);

			var patientSpecs = new PatientSpecifications(userId);
			var patient = await _unitOfWork.Repository<Patient>().GetWithSpecAsync(patientSpecs);

			var newMriScan = new MriScan()
			{
				Confidence = aiResponse.Confidence,
				AiGeneratedImagePath = aiResponse.Image,
				DetectionClass = aiResponse.Class,
				PatientId = patient.Id,
				ImagePath = fileUrl
			};

			foreach (var nearbyClinic in nearbyClinics)
			{
				var doctorId = nearbyClinic.Doctor.Id;

				if (!newMriScan.DoctorAssignments.Any(DC => DC.DoctorId == doctorId))
				{
					var newDoctorAssignment = new DoctorMriAssignment()
					{
						DoctorId = doctorId,
					};

					newMriScan.DoctorAssignments.Add(newDoctorAssignment);
				}

				if (newMriScan.DoctorAssignments.Count == 2) break;
			}

			_unitOfWork.Repository<MriScan>().Add(newMriScan);

			await _unitOfWork.CompleteAsync();

			return newMriScan;
		}
	}
}
