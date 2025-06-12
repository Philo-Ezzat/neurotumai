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

namespace NeuroTumAI.APIs.Controllers.Ticket
{
	[Authorize]
	public class TicketController : BaseApiController
	{
		private readonly ITicketService _ticketService;
        private readonly IMapper _mapper;


        public TicketController(ITicketService ticketService)
		{
			_ticketService = ticketService;
		}

        //[HttpGet]
        //public async Task<ActionResult> GetAllPosts()
        //{
        //    var posts = await _ticketService.GetPostsAsync(query =>
        //        query.Include(p => p.Comments)
        //             .Include(p => p.Likes)
        //             .Include(p => p.ApplicationUser)
        //    );
        //    if (posts == null)
        //        throw new UnAuthorizedException("No posts found.");

        //    return Ok(posts);
        //}


        [HttpGet("{ticketId}")]
        public async Task<ActionResult> GetTicketById(int ticketId)
        {

            var ticket = await _ticketService.GetTicketByIdAsync(ticketId);
            if (ticket is null)
                throw new UnAuthorizedException("Ticket not found.");


            //return Ok(new { Data = _mapper.Map<AdminToReturnDto>(post) });
            return Ok(ticket);
        }


        [HttpGet("byDoctorId/{doctorId}")]
        public async Task<ActionResult> GetTicketsByDoctorId(int doctorId)
        {

            var ticket = await _ticketService.GetTicketsByDoctorIdAsync(doctorId);
            if (ticket is null)
                throw new UnAuthorizedException("Ticket not found.");


            //return Ok(new { Data = _mapper.Map<AdminToReturnDto>(post) });
            return Ok(ticket);
        }


        [HttpGet("byPatientId/{patientId}")]
        public async Task<ActionResult> GetTicketsByPatientId(int patientId)
        {

            var ticket = await _ticketService.GetTicketsByPatientIdAsync(patientId);
            if (ticket is null)
                throw new UnAuthorizedException("Ticket not found.");


            //return Ok(new { Data = _mapper.Map<AdminToReturnDto>(post) });
            return Ok(ticket);
        }


        [HttpGet("byLabId/{labId}")]
        public async Task<ActionResult> GetTicketsByLabId(int labId)
        {

            var ticket = await _ticketService.GetTicketsByLabIdAsync(labId);
            if (ticket is null)
                throw new UnAuthorizedException("Ticket not found.");


            //return Ok(new { Data = _mapper.Map<AdminToReturnDto>(post) });
            return Ok(ticket);
        }


        [HttpPost]
        public async Task<ActionResult> AddTicket(AddTicketDto model)
        {
            var ticket = await _ticketService.AddTicketAsync(model);
            return Ok(ticket);
        }

        [HttpPut("updateLabId/{ticketId}")]
        public async Task<ActionResult> UpdateLabId(int ticketId, UpdateLabIdDto model)
        {

            await _ticketService.UpdateLabIdAsync(ticketId, model);

            return Ok();
        }

        [HttpPut("updateMRI/{ticketId}")]
        public async Task<ActionResult> UpdateMRI(int ticketId, PredictRequestDto model)
        {

            await _ticketService.UpdateMRIAsync(ticketId, model);

            return Ok();
        }

        [HttpPut("completeTicket/{ticketId}")]
        public async Task<ActionResult> CompleteTicket(int ticketId, CompleteTicketDto model)
        {

            await _ticketService.CompleteTicketAsync(ticketId, model);

            return Ok();
        }
        //[HttpPost("toggleLike/{postId}")]
        //public async Task<ActionResult<ToggleLikeResponseDto>> Togglike(int postId)
        //{
        //	var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        //	var result = await _ticketService.ToggleLikeAsync(userId, postId);

        //	return Ok(result);
        //}

        //      [HttpPost("addComment/{postId}")]
        //      public async Task<ActionResult<AddCommentResponseDto>> AddComment(int postId, AddCommentDto model)
        //      {
        //          var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        //          var result = await _ticketService.AddCommentAsync(userId, postId, model);

        //          return Ok(result);
        //      }
    }
}
