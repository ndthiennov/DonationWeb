using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DonationAppDemo.DAL
{
    public class PostDal : IPostDal
    {
        private readonly DonationDbContext _dbContext;

        public PostDal(DonationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Post post)
        {
            await _dbContext.Post.AddAsync(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Post post)
        {
            _dbContext.Post.Update(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Post post)
        {
            _dbContext.Post.Remove(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Post> GetByIdAsync(int id)
        {
            return await _dbContext.Post.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _dbContext.Post.ToListAsync();
        }
    }
}