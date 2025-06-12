using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DonationAppDemo.DAL
{
    public class CommentDal : ICommentDal
    {
        private readonly DonationDbContext _dbContext;

        public CommentDal(DonationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(CommentPost comment)
        {
            await _dbContext.CommentPost.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(CommentPost comment)
        {
            _dbContext.CommentPost.Update(comment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(CommentPost comment)
        {
            _dbContext.CommentPost.Remove(comment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CommentPost> GetByIdAsync(int id)
        {
            return await _dbContext.CommentPost.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<CommentPost>> GetByPostIdAsync(int postId)
        {
            return await _dbContext.CommentPost.Where(c => c.PostId == postId).ToListAsync();
        }
    }
}