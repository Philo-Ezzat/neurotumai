using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Lab;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.Post_Aggregate;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface ILabService
	{
        Task<IEnumerable<Lab>> GetLabsAsync(Func<IQueryable<Lab>, IQueryable<Lab>> include = null);
        //Task<Post> GetPostByIdAsync(int postId);
        Task<Lab> AddLabAsync(LabDto model);
        //Task<ToggleLikeResponseDto> ToggleLikeAsync(string userId, int postId);
        //Task<AddCommentResponseDto> AddCommentAsync(string userId, int postId, AddCommentDto model);
    }
}
