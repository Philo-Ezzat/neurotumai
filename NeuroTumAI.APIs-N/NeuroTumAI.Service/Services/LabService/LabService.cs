using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Dtos.Lab;
using NeuroTumAI.Core.Dtos.Post;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.Admin;
using NeuroTumAI.Core.Entities.Post_Aggregate;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Resources.Responses;
using NeuroTumAI.Core.Resources.Shared;
using NeuroTumAI.Core.Resources.Validation;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.ClinicSpecs;
using NeuroTumAI.Core.Specifications.PostSpecs.LikeSpecs;
using NeuroTumAI.Service.Hubs;
using NeuroTumAI.Service.Services.BlobStorageService;
using Newtonsoft.Json;

namespace NeuroTumAI.Service.Services.LabService
{
	public class LabService : ILabService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHubContext<PostHub> _hubContext;
		private readonly ILocalizationService _localizationService;
        private readonly UserManager<ApplicationUser> _userManager;


        public LabService(IUnitOfWork unitOfWork,IHubContext<PostHub> hubContext,ILocalizationService localizationService, UserManager<ApplicationUser> userManager)
		{
			_unitOfWork = unitOfWork;
			_hubContext = hubContext;
			_localizationService = localizationService;
            _userManager = userManager;

        }

        public async Task<IEnumerable<Lab>> GetLabsAsync(Func<IQueryable<Lab>, IQueryable<Lab>>? include = null)
        {
            var labRepo = _unitOfWork.Repository<Lab>();

            var labs = await labRepo.GetAllAsync();

            return labs;
        }



		//public async Task<Lab> GetTicketByLabIdAsync(int labId)
		//{
		//	var labRepo = _unitOfWork.Repository<Lab>();
		//	return await labRepo.GetAsync(labId);
		//}

		public async Task<Lab> AddLabAsync(LabDto model)
		{

            var newAccount = new ApplicationUser()
            {
                FullName = model.Name,
                UserName = model.Name,
                Email = model.Email,
				EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newAccount, model.Password);

            if (!result.Succeeded)
                throw new ValidationException(_localizationService.GetMessage<SharedResources>("ValidationError"))
                {
                    Errors = result.Errors.Select((E) => E.Description)
                };

            await _userManager.AddToRoleAsync(newAccount, "Lab");

            var newLab = new Lab()
            {
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                ApplicationUserId = newAccount.Id

            };

            var labRepo = _unitOfWork.Repository<Lab>();

            labRepo.Add(newLab);
            await _unitOfWork.CompleteAsync();


            Console.WriteLine(JsonConvert.SerializeObject(model));

			return newLab;
		}



		//      public async Task<ToggleLikeResponseDto> ToggleLikeAsync(string userId, int postId)
		//{
		//	var postRepo = _unitOfWork.Repository<Post>();
		//	var post = await postRepo.GetAsync(postId);

		//	if (post is null)
		//		throw new NotFoundException(_localizationService.GetMessage<ResponsesResources>("PostNotFound"));

		//	var likeRepo = _unitOfWork.Repository<Like>();

		//	var likeSpec = new LikeByUserAndPostSpecification(userId , postId);
		//	var existingLike = await likeRepo.GetWithSpecAsync(likeSpec);
		//	if (existingLike is not null)
		//	{
		//		likeRepo.Delete(existingLike);
		//		post.LikesCount--;
		//		postRepo.Update(post);
		//	} else
		//	{
		//		var newLike = new Like()
		//		{
		//			ApplicationUserId = userId,
		//			PostId = postId
		//		};
		//		likeRepo.Add(newLike);
		//		post.LikesCount++;
		//		postRepo.Update(post);
		//	}

		//	await _unitOfWork.CompleteAsync();
		//	await _hubContext.Clients.All.SendAsync("ReceivePostUpdate", new {
		//		PostId = postId,
		//		post.LikesCount,
		//		post.CommentsCount
		//	});

		//	return new ToggleLikeResponseDto()
		//	{
		//		IsLiked = existingLike is null,
		//		PostId = postId
		//	};
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
