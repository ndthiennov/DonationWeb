using DonationAppDemo.Models;
using System.Threading.Tasks;

namespace DonationAppDemo.DAL.Interfaces
{
    public interface IImagePostDal
    {
        Task AddAsync(ImagePost imagePost);
        Task UpdateAsync(ImagePost imagePost);
        Task DeleteByPostIdAsync(int postId);
        Task<ImagePost> GetByPostIdAsync(int postId);
    }
}