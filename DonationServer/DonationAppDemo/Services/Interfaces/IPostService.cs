using DonationAppDemo.DTOs;
using System.Threading.Tasks;

namespace DonationAppDemo.Services.Interfaces
{
    public interface IPostService
    {
        // Post APIs
        Task AddPostAsync(PostDto postDto);
        Task UpdatePostAsync(int id, PostDto postDto);
        Task DeletePostAsync(int id);
        Task<object> GetPostDetailAsync(int id);
        Task<object> GetPostsForAdminAsync(string postDate);
        Task<object> GetPostsForUserAsync(int page, int pageSize);

        // Comment APIs
        Task AddCommentAsync(CommentDto commentDto);
        Task UpdateCommentAsync(int id, CommentDto commentDto);
        Task DeleteCommentAsync(int id);
        Task<object> GetCommentsByPostAsync(int postId);
    }
}
