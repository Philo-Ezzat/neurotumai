using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.Account;
using NeuroTumAI.Core.Dtos.Admin;
using NeuroTumAI.Core.Dtos.Post;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Service.Dtos.Account;
using NeuroTumAI.Service.Services.AdminService;

namespace NeuroTumAI.APIs.Controllers.Post
{
	[Authorize]
	public class PostController : BaseApiController
	{
		private readonly IPostService _postService;
        private readonly IMapper _mapper;


        public PostController(IPostService postService)
		{
			_postService = postService;
		}

        [HttpGet]
        public async Task<ActionResult> GetAllPosts()
        {
            var posts = await _postService.GetPostsAsync(query =>
                query.Include(p => p.Comments)
                     .Include(p => p.Likes)
                     .Include(p => p.ApplicationUser)
            );
            if (posts == null)
                throw new UnAuthorizedException("No posts found.");

            return Ok(posts);
        }


        [HttpGet("{postId}")]
        public async Task<ActionResult> GetPost(int postId)
        {

            var post = await _postService.GetPostByIdAsync(postId);
            if (post is null)
                throw new UnAuthorizedException("Post not found.");


            //return Ok(new { Data = _mapper.Map<AdminToReturnDto>(post) });
            return Ok(post);
        }

        [HttpPost]
		public async Task<ActionResult> AddPost(AddPostDto model)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			await _postService.AddPostAsync(model, userId!);

			return Ok();
		}

		[HttpPost("toggleLike/{postId}")]
		public async Task<ActionResult<ToggleLikeResponseDto>> Togglike(int postId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			var result = await _postService.ToggleLikeAsync(userId, postId);

			return Ok(result);
		}

        [HttpPost("addComment/{postId}")]
        public async Task<ActionResult<AddCommentResponseDto>> AddComment(int postId, AddCommentDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var result = await _postService.AddCommentAsync(userId, postId, model);

            return Ok(result);
        }
    }
}
