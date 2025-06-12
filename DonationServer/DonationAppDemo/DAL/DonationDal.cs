using CloudinaryDotNet;
using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DonationAppDemo.DAL
{
    public class DonationDal : IDonationDal
    {
        private readonly DonationDbContext _context;

        public DonationDal(DonationDbContext context)
        {
            _context = context;
        }
        /*public async Task<List<Donation>?> GetListByCampaignId(int campaignId, int pageIndex, DateTime? fromDate, DateTime? toDate, int? donorId)
        {
            List<Donation>? donations = new List<Donation>();
            if (donorId != null)
            {
                if(fromDate != null || toDate != null)
                {
                    if(fromDate == null || toDate == null)
                    {
                        throw new Exception("Missing data");
                    }

                    donations = await _context.Donation
                        .Where(x => x.CampaignId == campaignId && x.DonorId == donorId && fromDate <= x.DonationDate && x.DonationDate <= toDate)
                        .Skip((pageIndex - 1) * 20)
                        .Take(20)
                        .ToListAsync();

                    return donations;
                }

                donations = await _context.Donation
                    .Where(x => x.CampaignId == campaignId && x.DonorId == donorId)
                    .Skip((pageIndex - 1) * 20)
                    .Take(20)
                    .ToListAsync();

                return donations;
            }

            if (fromDate != null || toDate != null)
            {
                if (fromDate == null || toDate == null)
                {
                    throw new Exception("Missing data");
                }

                donations = await _context.Donation
                    .Where(x => x.CampaignId == campaignId && fromDate <= x.DonationDate && x.DonationDate <= toDate)
                    .Skip((pageIndex - 1) * 20)
                    .Take(20)
                    .ToListAsync();

                return donations;
            }

            donations = await _context.Donation
                .Where(x => x.CampaignId == campaignId)
                .Skip((pageIndex - 1) * 20)
                .Take(20)
                .ToListAsync();

            return donations;
        }*/

        public async Task<List<DonationDto>?> GetSearchedListByCampaignId(int campaignId, SearchDto searchDto)
        {
            List<DonationDto>? donations = new List<DonationDto>();
            if (searchDto.OrderBy == "desc")
            {
                donations = await _context.Donation
                    .Join(_context.Donor,
                    donation => donation.DonorId,
                    donor => donor.Id,
                    (donation, donor) => new { donation, donor })
                    .Where(x => (x.donation.CampaignId == campaignId) &&
                    (x.donor.Id.ToString() == searchDto.Donor || (x.donor.NormalizedName != null && x.donor.NormalizedName.Contains(searchDto.Donor))) &&
                    ((searchDto.FromDate == "" || x.donation.DonationDate.Value.Date >= DateTime.Parse(searchDto.FromDate).Date) && (searchDto.ToDate == "" || x.donation.DonationDate.Value.Date <= DateTime.Parse(searchDto.ToDate).Date)))
                    .OrderByDescending(x => x.donation.Amount)
                    .Skip((searchDto.PageIndex - 1) * 10)
                    .Take(10)
                    .Select(x => new DonationDto
                    {
                        DonorId = x.donation.DonorId,
                        DonorName = x.donor.Name,
                        DonorAvaSrc = x.donor.AvaSrc,
                        Amount = x.donation.Amount,
                        DonationDate = x.donation.DonationDate == null ? "?" : x.donation.DonationDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                    })
                    .ToListAsync();
            }
            else if (searchDto.OrderBy == "asc")
            {
                donations = await _context.Donation
                    .Join(_context.Donor,
                    donation => donation.DonorId,
                    donor => donor.Id,
                    (donation, donor) => new { donation, donor })
                    .Where(x => (x.donation.CampaignId == campaignId) &&
                    (x.donor.Id.ToString() == searchDto.Donor || (x.donor.NormalizedName != null && x.donor.NormalizedName.Contains(searchDto.Donor))) &&
                    ((searchDto.FromDate == "" || x.donation.DonationDate.Value.Date >= DateTime.Parse(searchDto.FromDate).Date) && (searchDto.ToDate == "" || x.donation.DonationDate.Value.Date <= DateTime.Parse(searchDto.ToDate).Date)))
                    .OrderBy(x => x.donation.Amount)
                    .Skip((searchDto.PageIndex - 1) * 10)
                    .Take(10)
                    .Select(x => new DonationDto
                    {
                        DonorId = x.donation.DonorId,
                        DonorName = x.donor.Name,
                        DonorAvaSrc = x.donor.AvaSrc,
                        Amount = x.donation.Amount,
                        DonationDate = x.donation.DonationDate == null ? "?" : x.donation.DonationDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                    })
                    .ToListAsync();
            }
            else
            {
                donations = await _context.Donation
                    .Join(_context.Donor,
                    donation => donation.DonorId,
                    donor => donor.Id,
                    (donation, donor) => new { donation, donor })
                    .Where(x => (x.donation.CampaignId == campaignId) &&
                    (x.donor.Id.ToString() == searchDto.Donor || (x.donor.NormalizedName != null && x.donor.NormalizedName.Contains(searchDto.Donor))) &&
                    ((searchDto.FromDate == "" || x.donation.DonationDate.Value.Date >= DateTime.Parse(searchDto.FromDate).Date) && (searchDto.ToDate == "" || x.donation.DonationDate.Value.Date <= DateTime.Parse(searchDto.ToDate).Date)))
                    .Skip((searchDto.PageIndex - 1) * 10)
                    .Take(10)
                    .Select(x => new DonationDto
                    {
                        DonorId = x.donation.DonorId,
                        DonorName = x.donor.Name,
                        DonorAvaSrc = x.donor.AvaSrc,
                        Amount = x.donation.Amount,
                        DonationDate = x.donation.DonationDate == null ? "?" : x.donation.DonationDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                    })
                    .ToListAsync();
            }
            
            return donations;
        }
        /*public async Task<List<DonationDto>?> GetListByDonorId(int donorId, int pageIndex, DateTime? fromDate, DateTime? toDate)
        {
            List<DonationDto>? donations = new List<DonationDto>();

            if (fromDate != null || toDate != null)
            {
                if (fromDate == null || toDate == null)
                {
                    throw new Exception("Missing data");
                }

                donations = await _context.Donation
                    .Where(x => x.DonorId == donorId && fromDate <= x.DonationDate && x.DonationDate <= toDate)
                    .Skip((pageIndex - 1) * 20)
                    .Take(20)
                    .Join(_context.Campaign, donation => donation.CampaignId, campaign => campaign.Id,
                    (donation, campaign) => new DonationDto()
                    {
                        CampaignId = campaign.Id,
                        CampaignName = campaign.Title,
                        Amount = donation.Amount,
                        DonationDate = donation.DonationDate
                    })
                    .ToListAsync();

                return donations;
            }

            donations = await _context.Donation
                    .Where(x => x.DonorId == donorId)
                    .Skip((pageIndex - 1) * 20)
                    .Take(20)
                    .Join(_context.Campaign, donation => donation.CampaignId, campaign => campaign.Id,
                    (donation, campaign) => new DonationDto()
                    {
                        CampaignId = campaign.Id,
                        CampaignName = campaign.Title,
                        Amount = donation.Amount,
                        DonationDate = donation.DonationDate
                    })
                    .ToListAsync();

            return donations;
        }*/
        public async Task<Donation> Add(PaymentResponseDto responseDto)
        {
            var donation = new Donation()
            {
                DonationDate = responseDto.PaymentDate,
                Amount = responseDto.Amount,
                DonorId = responseDto.UserId,
                CampaignId = responseDto.CampaignId,
                PaymentMethodId = responseDto.PaymentMethodId,
                PaymentDescription = responseDto.PaymentDescription,
                PaymentOrderId = responseDto.PaymentOrderId,
                PaymentTransactionId = responseDto.PaymentTransactionId,
                PaymentToken = responseDto.PaymentToken,
                PaymentResponse = "sucess",
            };
            _context.Donation.Add(donation);
            await _context.SaveChangesAsync();
            return donation;
        }
    }
}
