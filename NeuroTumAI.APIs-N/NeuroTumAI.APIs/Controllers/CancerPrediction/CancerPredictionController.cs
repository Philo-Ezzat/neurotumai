using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.CancerPrediction;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Service.Services.BlobStorageService;

namespace NeuroTumAI.APIs.Controllers.CancerPrediction
{
	public class CancerPredictionController : BaseApiController
	{
		private readonly ICancerDetectionService _cancerDetectionService;
		private readonly IBlobStorageService _blobStorageService;

		public CancerPredictionController(ICancerDetectionService cancerDetectionService, IBlobStorageService blobStorageService)
		{
			_cancerDetectionService = cancerDetectionService;
			_blobStorageService = blobStorageService;
		}

		[HttpPost]
		public async Task<ActionResult<CancerPredictionResultDto>> PredictCancer([FromForm] PredictRequestDto model)
		{
			using var stream = model.Image.OpenReadStream();
			var fileUrl = await _blobStorageService.UploadFileAsync(stream, model.Image.FileName, "patient-cancer-images");

			var result = await _cancerDetectionService.PredictCancerAsync(fileUrl);

			return Ok(result);
		}
	}
}
