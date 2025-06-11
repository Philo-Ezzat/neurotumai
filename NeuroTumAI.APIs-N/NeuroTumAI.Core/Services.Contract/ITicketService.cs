using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.CancerPrediction;
using NeuroTumAI.Core.Dtos.Post;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.Post_Aggregate;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface ITicketService
	{
        //Task<IEnumerable<Post>> GetPostsAsync(Func<IQueryable<Post>, IQueryable<Post>> include = null);
        Task<Ticket> GetTicketByIdAsync(int id);
        Task<IReadOnlyList<Ticket>> GetTicketsByLabIdAsync(int labId);
        Task<IReadOnlyList<Ticket>> GetTicketsByDoctorIdAsync(int doctorId);
        Task<IReadOnlyList<Ticket>> GetTicketsByPatientIdAsync(int patientId);
        Task<Ticket> AddTicketAsync(AddTicketDto model);
        Task<Ticket> UpdateLabIdAsync(int ticketId, UpdateLabIdDto model);
        Task<Ticket> UpdateMRIAsync(int ticketId, PredictRequestDto model);
        Task<Ticket> CompleteTicketAsync(int ticketId, CompleteTicketDto model);

        //Task<ToggleLikeResponseDto> ToggleLikeAsync(string userId, int postId);
        //Task<AddCommentResponseDto> AddCommentAsync(string userId, int postId, AddCommentDto model);
    }
}
