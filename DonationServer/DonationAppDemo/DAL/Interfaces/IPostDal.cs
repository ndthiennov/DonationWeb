using DonationAppDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DonationAppDemo.DAL.Interfaces
{
    public interface IPostDal
    {
        Task AddAsync(Post post);
        Task UpdateAsync(Post post);
        Task DeleteAsync(Post post);
        Task<Post> GetByIdAsync(int id);
        Task<IEnumerable<Post>> GetAllAsync();
    }
}