using DonationAppDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DonationAppDemo.DAL.Interfaces
{
    public interface ICommentDal
    {
        Task AddAsync(CommentPost comment);
        Task UpdateAsync(CommentPost comment);
        Task DeleteAsync(CommentPost comment);
        Task<CommentPost> GetByIdAsync(int id);
        Task<IEnumerable<CommentPost>> GetByPostIdAsync(int postId);
    }
}