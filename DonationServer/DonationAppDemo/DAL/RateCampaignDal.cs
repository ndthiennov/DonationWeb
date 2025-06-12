using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DonationAppDemo.DAL
{
    public class RateCampaignDal: IRateCampaignDal
    {
        private readonly DonationDbContext _context;
        public RateCampaignDal(DonationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RateCampaignDto>> GetListByCampaignId(int campaignId, int pageIndex)
        {
            // Fetch all image campaigns that match the given campaignId
            return await _context.RateCampaign
                .Join(_context.Donor,
                    rateCampaign => rateCampaign.DonorId,
                    donor => donor.Id,
                    (rateCampaign, donor) => new { rateCampaign, donor })
                .Where(x => x.rateCampaign.CampaignId == campaignId)
                .OrderByDescending(x => x.rateCampaign.RatedDate)
                .Skip((pageIndex - 1) * 5)
                .Take(5)
                .Select(x => new RateCampaignDto
                {
                    CampaignId = x.rateCampaign.CampaignId,
                    DonorId = x.rateCampaign.DonorId,
                    DonorName = x.donor.Name,
                    DonorAva = x.donor.AvaSrc,
                    Rate = x.rateCampaign.Rate,
                    Content = x.rateCampaign.Comment,
                    RatedDate = x.rateCampaign.RatedDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                })
                .ToListAsync();
        }
        public async Task<bool> Add(RateCampaign rateCampaign)
        {
            _context.RateCampaign.Add(rateCampaign);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> RemoveByDonorId(int campaignId, int donorId)
        {
            var rate = await _context.RateCampaign.Where(x => x.CampaignId == campaignId && x.DonorId == donorId).FirstOrDefaultAsync();
            if(rate == null) return false;

            _context.RateCampaign.Remove(rate);
            await _context.SaveChangesAsync();

            return true;
        }
        /*private readonly DonationDbContext _context;
        public RateCampaignDal(DonationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckExistedRateCampaign(RateCampaignDto rateCampaignDto)
        {
            //check if the donor rated campaign
            var existingRateCampaign =  await _context.RateCampaign.Where(x => x.CampaignId == rateCampaignDto.CampaignId && x.DonorId == rateCampaignDto.DonorId).FirstOrDefaultAsync();
            if (existingRateCampaign != null)
            {
                return true;
            }
            return false;
        }

        public async Task<RateCampaign> Add(RateCampaign rateCampaign)
        {
            _context.RateCampaign.Add(rateCampaign);
            await _context.SaveChangesAsync();
            return rateCampaign;
        }
        public async Task<RateCampaign> Update(RateCampaignDto rateCampaignDto)
        {
            //check if user has rated the campaign
            var existedRateCampaign = await _context.RateCampaign.Where(x => x.CampaignId == rateCampaignDto.CampaignId && x.DonorId == rateCampaignDto.DonorId).FirstOrDefaultAsync();
            if(existedRateCampaign != null)
            {
                existedRateCampaign.Rate = rateCampaignDto.Rate;
                existedRateCampaign.Comment = rateCampaignDto.Content;
                _context.RateCampaign.Update(existedRateCampaign);
                await _context.SaveChangesAsync();
            }
            return existedRateCampaign;
        }

        public async Task<bool> RemoveByCampaignId(int campaignId)
        {
            List<RateCampaign> listRateCampaign = await _context.RateCampaign.Where(x => x.CampaignId == campaignId).ToListAsync();
            if (listRateCampaign == null)
            {
                return false;
            }
            foreach (var rate in listRateCampaign)
            {
                _context.RateCampaign.Remove(rate);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<RateCampaign>> GetById(int campaignId, int pageSize, int pageIndex)
        {
            // Fetch all image campaigns that match the given campaignId
            return await _context.RateCampaign
                                    .Where(x => x.CampaignId == campaignId)
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
        }
        public async Task<List<RateCampaign>> GetAllById(int campaignId)
        {
            // Fetch all image campaigns that match the given campaignId
            return await _context.RateCampaign
                                    .Where(x => x.CampaignId == campaignId)
                                    .ToListAsync();
        }*/
    }
}