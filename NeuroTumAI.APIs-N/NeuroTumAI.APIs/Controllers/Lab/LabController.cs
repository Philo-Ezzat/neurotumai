using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeuroTumAI.APIs.Controllers.Base;
using NeuroTumAI.Core.Dtos.Account;
using NeuroTumAI.Core.Dtos.Admin;
using NeuroTumAI.Core.Dtos.CancerPrediction;
using NeuroTumAI.Core.Dtos.Clinic;
using NeuroTumAI.Core.Dtos.Post;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Service.Dtos.Account;
using NeuroTumAI.Service.Services.AdminService;
using NeuroTumAI.Service.Services.ClinicService;

namespace NeuroTumAI.APIs.Controllers.Lab
{
	[Authorize]
	public class LabController : BaseApiController
	{
		private readonly ILabService _labService;
        private readonly IMapper _mapper;


        public LabController(ILabService labService)
		{
			_labService = labService;
		}

        //[HttpGet]
        //public async Task<ActionResult> GetAllPosts()
        //{
        //    var posts = await _labService.GetPostsAsync(query =>
        //        query.Include(p => p.Comments)
        //             .Include(p => p.Likes)
        //             .Include(p => p.ApplicationUser)
        //    );
        //    if (posts == null)
        //        throw new UnAuthorizedException("No posts found.");

        //    return Ok(posts);
        //}


        [HttpGet()]
        public async Task<ActionResult> GetLabs()
        {

            var labs = await _labService.GetLabsAsync();

            return Ok(labs);
        }
    }
}
