using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DonationAppDemo.DAL
{
    public class ImagePostDal : IImagePostDal
    {
        private readonly DonationDbContext _dbContext;

        public ImagePostDal(DonationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(ImagePost imagePost)
        {
            await _dbContext.ImagePost.AddAsync(imagePost);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(ImagePost imagePost)
        {
            _dbContext.ImagePost.Update(imagePost);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteByPostIdAsync(int postId)
        {
            var imagePost = await _dbContext.ImagePost.FirstOrDefaultAsync(i => i.PostId == postId);
            if (imagePost != null)
            {
                _dbContext.ImagePost.Remove(imagePost);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<ImagePost> GetByPostIdAsync(int postId)
        {
            return await _dbContext.ImagePost.FirstOrDefaultAsync(i => i.PostId == postId);
        }
    }
}