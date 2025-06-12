using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DonationAppDemo.DAL
{
    public class CampaignParticipantDal : ICampaignParticipantDal
    {
        private readonly DonationDbContext _context;

        public CampaignParticipantDal(DonationDbContext context)
        {
            _context = context;
        }
        public async Task<bool>CheckParticipated(int donorId, int campaignId)
        {
            var result = await _context.CampaignParticipant.Where(x => x.CampaignId == campaignId && x.DonorId == donorId).FirstOrDefaultAsync();
            if(result == null)
            {
                return false;
            }
            return true;
        }
        public async Task<List<int>?> GetAllDonorIdByCampaignId(int campaignId)
        {
            var userIds = await _context.CampaignParticipant.Where(x => x.CampaignId == campaignId).Select(x => x.DonorId).ToListAsync();
            return userIds;
        }
        public async Task<bool> JoinCampaign(int campaignId, int donorId)
        {
            var campaignParticipant = new CampaignParticipant()
            {
                CampaignId = campaignId,
                DonorId = donorId
            };
            _context.CampaignParticipant.Add(campaignParticipant);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CancelCampaignPartipation(int campaignId, int donorId)
        {
            var campaignParticipant = await _context.CampaignParticipant.Where(x => x.CampaignId == campaignId && x.DonorId == donorId).FirstOrDefaultAsync();
            if (campaignParticipant == null)
            {
                throw new Exception($"Not found donor id {donorId} in campaign id {campaignId}");
            }

            _context.CampaignParticipant.Remove(campaignParticipant);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
