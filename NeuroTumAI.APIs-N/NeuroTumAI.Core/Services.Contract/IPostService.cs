using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Post;
using NeuroTumAI.Core.Entities.Post_Aggregate;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IPostService
	{
        Task<IEnumerable<Post>> GetPostsAsync(Func<IQueryable<Post>, IQueryable<Post>> include = null);
        Task<Post> GetPostByIdAsync(int postId);
        Task<Post> AddPostAsync(AddPostDto model, string applicationUserId);
        Task<ToggleLikeResponseDto> ToggleLikeAsync(string userId, int postId);
        Task<AddCommentResponseDto> AddCommentAsync(string userId, int postId, AddCommentDto model);
    }
}
