using System.Net.Http.Headers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Dtos.CancerPrediction;
using NeuroTumAI.Core.Dtos.Notification;
using NeuroTumAI.Core.Dtos.Post;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.Admin;
using NeuroTumAI.Core.Entities.Appointment;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;
using NeuroTumAI.Core.Entities.MriScan;
using NeuroTumAI.Core.Entities.Post_Aggregate;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Resources.Responses;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.AppointmentSpecs;
using NeuroTumAI.Core.Specifications.ClinicSpecs;
using NeuroTumAI.Core.Specifications.DoctorSpecs;
using NeuroTumAI.Core.Specifications.PatientSpecs;
using NeuroTumAI.Core.Specifications.PostSpecs.LikeSpecs;
using NeuroTumAI.Core.Specifications.TicketSpecs;
using NeuroTumAI.Service.Hubs;
using NeuroTumAI.Service.Services.BlobStorageService;
using NeuroTumAI.Service.Services.CancerDetectionService;
using NeuroTumAI.Service.Services.NotificationService;
using Newtonsoft.Json;

namespace NeuroTumAI.Service.Services.TicketService
{
	public class TicketService : ITicketService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHubContext<PostHub> _hubContext;
		private readonly ILocalizationService _localizationService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ICancerDetectionService _cancerDetectionService;

        public TicketService(
            IUnitOfWork unitOfWork,
            IHubContext<PostHub> hubContext,
            ILocalizationService localizationService,
            IBlobStorageService blobStorageService,
            ICancerDetectionService cancerDetectionService)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _localizationService = localizationService;
            _blobStorageService = blobStorageService;
            _cancerDetectionService = cancerDetectionService;
        }


        //public async Task<IEnumerable<Post>> GetPostsAsync(Func<IQueryable<Post>, IQueryable<Post>>? include = null)
        //{
        //    var postRepo = _unitOfWork.Repository<Post>();

        //    var posts = await postRepo.GetAllAsync(include);

        //    return posts;
        //}



        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            var ticketRepo = _unitOfWork.Repository<Ticket>();
            return await ticketRepo.GetAsync(id);
        }

        public async Task<IReadOnlyList<Ticket>> GetTicketsByLabIdAsync(int labId)
        {
            var ticketRepo = _unitOfWork.Repository<Ticket>();
            var spec = new TicketSpecifications(labId);
            return await ticketRepo.GetAllWithSpecAsync(spec);
        }

        public async Task<IReadOnlyList<Ticket>> GetTicketsByDoctorIdAsync(int doctorId)
        {
            var ticketRepo = _unitOfWork.Repository<Ticket>();
            var spec = new TicketSpecifications(0).ByDoctorId(doctorId); // 0 is a dummy labId, will be overwritten
            return await ticketRepo.GetAllWithSpecAsync(spec);
        }

        public async Task<IReadOnlyList<Ticket>> GetTicketsByPatientIdAsync(int patientId)
        {
            var ticketRepo = _unitOfWork.Repository<Ticket>();
            var spec = new TicketSpecifications(0).ByPatientId(patientId); // 0 is a dummy labId, will be overwritten
            return await ticketRepo.GetAllWithSpecAsync(spec);
        }

        public async Task<Ticket> AddTicketAsync(AddTicketDto model)
        {
            // Get the Doctor entity where ApplicationUserId == model.DoctorId
            var doctorRepo = _unitOfWork.Repository<Doctor>();
            var doctor = await doctorRepo.GetAllAsync(q => q.Where(d => d.ApplicationUserId == model.DoctorId));
            var doctorEntity = doctor.FirstOrDefault();
            if (doctorEntity == null)
                throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("DoctorNotFound"));

            // Get the Patient entity where ApplicationUserId == model.PatientId
            var patientRepo = _unitOfWork.Repository<Patient>();
            var patient = await patientRepo.GetAllAsync(q => q.Where(d => d.ApplicationUserId == model.PatientId));
            var patientEntity = patient.FirstOrDefault();
            if (patientEntity == null)
                throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("PatientNotFound"));

            var newTicket = new Ticket()
            {
                DoctorId = doctorEntity.Id,
                PatientId = patientEntity.Id,
                DrInitialDesc = model.DrInitialDesc,
                NeedMRI = model.NeedMRI,
            };

            var ticketRepo = _unitOfWork.Repository<Ticket>();
            ticketRepo.Add(newTicket);

            // Save to get the generated Ticket Id
            await _unitOfWork.CompleteAsync();

            var appRepo = _unitOfWork.Repository<Appointment>();
            if (!int.TryParse(model.AppId, out int appId))
                throw new ArgumentException("Invalid AppId format", nameof(model.AppId));

            var app = await appRepo.GetAsync(appId);
            if (app is null)
                throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("AppointmentNotFound"));

            app.TicketId = newTicket.Id;
            appRepo.Update(app);

            await _unitOfWork.CompleteAsync();

            Console.WriteLine(JsonConvert.SerializeObject(model));

            return newTicket;
        }


        public async Task<Ticket> UpdateLabIdAsync(int ticketId, UpdateLabIdDto model)
        {
            var ticketRepo = _unitOfWork.Repository<Ticket>();
            var ticket = await ticketRepo.GetAsync(ticketId);
            if (ticket is null)
                throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("TicketNotFound"));

            ticket.LabId = model.LabId;
            ticket.LabDate = model.LabDate;
            ticketRepo.Update(ticket);

            await _unitOfWork.CompleteAsync();
            return ticket;

        }

        public async Task<Ticket> UpdateMRIAsync(int ticketId, PredictRequestDto model)
        {
            using var stream = model.Image.OpenReadStream();
            var fileUrl = await _blobStorageService.UploadFileAsync(stream, model.Image.FileName, "patient-cancer-images");

            var aiResponse = await _cancerDetectionService.PredictCancerAsync(fileUrl);

            var ticketRepo = _unitOfWork.Repository<Ticket>();
            var ticket = await ticketRepo.GetAsync(ticketId);
            if (ticket is null)
                throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("TicketNotFound"));

            ticket.ImagePath = fileUrl;
            ticket.Status = TicketStatus.Reviewed;
            ticketRepo.Update(ticket);

            await _unitOfWork.CompleteAsync();
            return ticket;
        }


        public async Task<Ticket> UpdatePDFAsync(int ticketId, PredictRequestDto model)
        {
            using var form = new MultipartFormDataContent();

            // Prepare the image file content
            using var stream = model.Image.OpenReadStream();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.Image.ContentType);

            // Add image to form-data (key should match what the prediction API expects)
            form.Add(fileContent, "images", model.Image.FileName);

            // Send form to prediction service
            var pdfBytes = await _cancerDetectionService.PredictCancerPDFAsync(form);


            // Convert byte[] to Stream
            using var pdfStream = new MemoryStream(pdfBytes);

            // Generate a dynamic filename for the PDF
            var pdfFileName = $"cancer-prediction-{ticketId}-{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";

            // Upload the PDF to blob storage
            var pdfUrl = await _blobStorageService.UploadFileAsync(pdfStream, pdfFileName, "patient-cancer-reports");

            // Update the ticket
            var ticketRepo = _unitOfWork.Repository<Ticket>();
            var ticket = await ticketRepo.GetAsync(ticketId);
            if (ticket is null)
                throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("TicketNotFound"));

            ticket.AiGeneratedFilePath = pdfUrl;
            ticket.Status = TicketStatus.Reviewed;
            ticketRepo.Update(ticket);

            await _unitOfWork.CompleteAsync();
            return ticket; 
        }


        public async Task<Ticket> CompleteTicketAsync(int ticketId, CompleteTicketDto model)
        {
            var ticketRepo = _unitOfWork.Repository<Ticket>();
            var ticket = await ticketRepo.GetAsync(ticketId);
            if (ticket is null)
                throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("TicketNotFound"));

            ticket.DrFinalDesc = model.DrFinalDesc;
            ticket.Status = TicketStatus.Completed;
            ticketRepo.Update(ticket);

            await _unitOfWork.CompleteAsync();
            return ticket;

        }


        //public async Task<ToggleLikeResponseDto> ToggleLikeAsync(string userId, int postId)
        //{
        //    var postRepo = _unitOfWork.Repository<Post>();
        //    var post = await postRepo.GetAsync(postId);

        //    if (post is null)
        //        throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("PostNotFound"));

        //    var likeRepo = _unitOfWork.Repository<Like>();

        //    var likeSpec = new LikeByUserAndPostSpecification(userId, postId);
        //    var existingLike = await likeRepo.GetWithSpecAsync(likeSpec);
        //    if (existingLike is not null)
        //    {
        //        likeRepo.Delete(existingLike);
        //        post.LikesCount--;
        //        postRepo.Update(post);
        //    }
        //    else
        //    {
        //        var newLike = new Like()
        //        {
        //            ApplicationUserId = userId,
        //            PostId = postId
        //        };
        //        likeRepo.Add(newLike);
        //        post.LikesCount++;
        //        postRepo.Update(post);
        //    }

        //    await _unitOfWork.CompleteAsync();
        //    await _hubContext.Clients.All.SendAsync("ReceivePostUpdate", new
        //    {
        //        PostId = postId,
        //        post.LikesCount,
        //        post.CommentsCount
        //    });

        //    return new ToggleLikeResponseDto()
        //    {
        //        IsLiked = existingLike is null,
        //        PostId = postId
        //    };
        //}

        //      public async Task<AddCommentResponseDto> AddCommentAsync(string userId, int postId, AddCommentDto model)
        //      {
        //          var postRepo = _unitOfWork.Repository<Post>();
        //          var post = await postRepo.GetAsync(postId);

        //          if (post is null)
        //              throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("PostNotFound"));

        //          var commentRepo = _unitOfWork.Repository<Comment>();


        //	var newComment = new Comment()
        //	{
        //		ApplicationUserId = userId,
        //		PostId = postId,
        //		Text = model.Comment
        //	};
        //          commentRepo.Add(newComment);
        //          post.Comments.Add(newComment);
        //          post.CommentsCount++;
        //	postRepo.Update(post);


        //          await _unitOfWork.CompleteAsync();
        //          await _hubContext.Clients.All.SendAsync("ReceivePostUpdate", new
        //          {
        //              PostId = postId,
        //              post.LikesCount,
        //              post.CommentsCount,
        //		post.Comments
        //          });

        //          return new AddCommentResponseDto()
        //          {
        //              Comment = model.Comment,
        //              PostId = postId
        //          };
        //      }
    }
}
