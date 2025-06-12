using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DonationAppDemo.DAL
{
    public class TransferenceDal : ITransferenceDal
    {
        private readonly DonationDbContext _dbContext;

        public TransferenceDal(DonationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Transference transference)
        {
            await _dbContext.Transference.AddAsync(transference);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Transference transference)
        {
            _dbContext.Transference.Update(transference);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Transference transference)
        {
            _dbContext.Transference.Remove(transference);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Transference> GetByIdAsync(int id)
        {
            return await _dbContext.Transference.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transference>> GetByCampaignIdAsync(int campaignId)
        {
            return await _dbContext.Transference.Where(t => t.CampaignId == campaignId).ToListAsync();
        }
    }
}