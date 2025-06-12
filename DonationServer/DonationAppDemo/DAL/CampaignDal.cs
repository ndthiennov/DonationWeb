using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Helper;
using DonationAppDemo.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;
using Twilio.TwiML.Fax;

namespace DonationAppDemo.DAL
{
    public class CampaignDal : ICampaignDal
    {
        private readonly DonationDbContext _context;
        public CampaignDal(DonationDbContext context)
        {
            _context = context;
        }
        public async Task<List<CampaignShortADto>?> GetListByAdmin(int pageIndex)
        {
            var campaigns = await _context.Campaign
                .Skip((pageIndex - 1) * 20)
                .Take(20)
                .GroupJoin(_context.CampaignStatistics,
                    campaign => campaign.Id,
                    statistics => statistics.CampaignId,
                    (campaign, statistics) => new { campaign, statistics })
                .SelectMany(x => x.statistics.DefaultIfEmpty(),
                    (x, statistics) => new { x.campaign, statistics })
                .GroupJoin(_context.Organiser,
                    combined => combined.campaign.OrganiserId,
                    organiser => organiser.Id,
                    (combined, organiser) => new { combined.campaign, combined.statistics, organiser })
                .SelectMany(x => x.organiser.DefaultIfEmpty(),
                    (x, organiser) => new { x.campaign, x.statistics, organiser })
                .GroupJoin(_context.Recipient,
                    combined => combined.campaign.RecipientId,
                    recipient => recipient.Id,
                    (combined, recipient) => new { combined.campaign, combined.statistics, combined.organiser, recipient })
                .SelectMany(x => x.recipient.DefaultIfEmpty(),
                    (x, recipient) => new { x.campaign, x.statistics, x.organiser, recipient })
                //.OrderByDescending(x => x.campaign.CreatedDate)
                .Select(x => new CampaignShortADto
                {
                    Id = x.campaign.Id,
                    Title = x.campaign.Title,
                    //Target = x.campaign.Target,
                    StartDate = x.campaign.StartDate == null ? "?" : x.campaign.StartDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    EndDate = x.campaign.EndDate == null ? "?" : x.campaign.EndDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    City = x.campaign.City,
                    ReceivedTotal = x.statistics.TotalDonationAmount,
                    SpentTotal = x.statistics.TotalExpendedAmount,
                    Disabled = x.campaign.Disabled == false ? "Disabled" : "Active",
                    OrganiserId = x.organiser.Id,
                    OrganiserName = x.organiser.Name,
                    RecipientId = x.recipient.Id,
                    RecipientName = x.recipient.Name,
                    Received = x.campaign.Received == false ? "Not Received" : "Received"
                })
                .ToListAsync();
            return campaigns;
        }
        public async Task<List<CampaignShortADto>?> GetSearchedListByAdmin(int pageIndex, CampaignSearchADto search)
        {
            var campaigns = await _context.Campaign
                .GroupJoin(_context.CampaignStatistics,
                    campaign => campaign.Id,
                    statistics => statistics.CampaignId,
                    (campaign, statistics) => new { campaign, statistics })
                .SelectMany(x => x.statistics.DefaultIfEmpty(),
                    (x, statistics) => new { x.campaign, statistics })
                .GroupJoin(_context.Organiser,
                    combined => combined.campaign.OrganiserId,
                    organiser => organiser.Id,
                    (combined, organiser) => new { combined.campaign, combined.statistics, organiser })
                .SelectMany(x => x.organiser.DefaultIfEmpty(),
                    (x, organiser) => new { x.campaign, x.statistics, organiser })
                .GroupJoin(_context.Recipient,
                    combined => combined.campaign.RecipientId,
                    recipient => recipient.Id,
                    (combined, recipient) => new { combined.campaign, combined.statistics, combined.organiser, recipient })
                .SelectMany(x => x.recipient.DefaultIfEmpty(),
                    (x, recipient) => new { x.campaign, x.statistics, x.organiser, recipient })
                .Where(x => (x.campaign.Id.ToString() == search.Campaign || (x.campaign.NormalizedTitle != null && x.campaign.NormalizedTitle.Contains(search.Campaign))) &&
                    (x.organiser.Id.ToString() == search.User || (x.organiser.NormalizedName != null && x.organiser.NormalizedName.Contains(search.User))) &&
                    ((search.StartDate == "" || x.campaign.StartDate.Value.Date >= DateTime.Parse(search.StartDate).Date) && (search.EndDate == "" || x.campaign.EndDate.Value.Date <= DateTime.Parse(search.EndDate).Date)) &&
                    (x.campaign.City != null && x.campaign.City.Contains(search.City)))
                .Skip((pageIndex - 1) * 20)
                .Take(20)
                .OrderByDescending(x => x.campaign.CreatedDate)
                .Select(x => new CampaignShortADto
                {
                    Id = x.campaign.Id,
                    Title = x.campaign.Title,
                    StartDate = x.campaign.StartDate == null ? "?" : x.campaign.StartDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    EndDate = x.campaign.EndDate == null ? "?" : x.campaign.EndDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    City = x.campaign.City,
                    ReceivedTotal = x.statistics.TotalDonationAmount,
                    SpentTotal = x.statistics.TotalExpendedAmount,
                    Disabled = x.campaign.Disabled == false ? "Active" : "Disabled",
                    OrganiserId = x.organiser.Id,
                    OrganiserName = x.organiser.Name,
                    RecipientId = x.recipient.Id,
                    RecipientName = x.recipient.Name,
                    Received = x.campaign.Received == false ? "Unreceived" : "Received"
                })
                .ToListAsync();
            return campaigns;
        }
        public async Task<List<CampaignShortBDto>?> GetSearchedListByUser(int pageIndex, CampaignSearchADto search)
        {
            var campaigns = await _context.Campaign
                .GroupJoin(_context.StatusCampaign,
                    campaign => campaign.StatusCampaignId,
                    status => status.Id,
                    (campaign, status) => new { campaign, status })
                .SelectMany(x => x.status.DefaultIfEmpty(),
                    (x, status) => new { x.campaign, status })
                .GroupJoin(_context.Organiser,
                    combined => combined.campaign.OrganiserId,
                    organiser => organiser.Id,
                    (combined, organiser) => new { combined.campaign, combined.status, organiser })
                .SelectMany(x => x.organiser.DefaultIfEmpty(),
                    (x, organiser) => new { x.campaign, x.status, organiser })
                .GroupJoin(_context.Recipient,
                    combined => combined.campaign.RecipientId,
                    recipient => recipient.Id,
                    (combined, recipient) => new { combined.campaign, combined.status, combined.organiser, recipient })
                .SelectMany(x => x.recipient.DefaultIfEmpty(),
                    (x, recipient) => new { x.campaign, x.status, x.organiser, recipient })
                .Where(x => x.campaign.Disabled == false && (x.campaign.Id.ToString() == search.Campaign || (x.campaign.NormalizedTitle != null && x.campaign.NormalizedTitle.Contains(search.Campaign))) &&
                    (x.organiser.Id.ToString() == search.User || (x.organiser.NormalizedName != null && x.organiser.NormalizedName.Contains(search.User))) &&
                    ((search.StartDate == "" || x.campaign.StartDate.Value.Date >= DateTime.Parse(search.StartDate).Date) && (search.EndDate == "" || x.campaign.EndDate.Value.Date <= DateTime.Parse(search.EndDate).Date)) &&
                    (x.campaign.City != null && x.campaign.City.Contains(search.City)))
                .Skip((pageIndex - 1) * 20)
                .Take(20)
                .OrderByDescending(x => x.campaign.CreatedDate)
                .Select(x => new CampaignShortBDto
                {
                    Id = x.campaign.Id,
                    Title = x.campaign.Title,
                    Target = x.campaign.Target,
                    StartDate = x.campaign.StartDate == null ? "?" : x.campaign.StartDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    EndDate = x.campaign.EndDate == null ? "?" : x.campaign.EndDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Address = x.campaign.Address + ", " + x.campaign.City,
                    CoverSrc = x.campaign.CoverSrc,
                    Status = x.status.Name,
                    OrganiserId = x.organiser.Id,
                    OrganiserName = x.organiser.Name,
                    OrganiserAva = x.organiser.AvaSrc,
                    RecipientId = x.recipient.Id,
                    RecipientName = x.recipient.Name,
                    Received = x.campaign.Received == false ? "Chưa nhận" : "Đã nhận"
                })
                .ToListAsync();
            return campaigns;
        }
        public async Task<List<CampaignShortCDto>?> GetSearchedListByOrganiser(int pageIndex, CampaignSearchADto search, int organiserId)
        {
            var campaigns = await _context.Campaign
                .GroupJoin(_context.StatusCampaign,
                    campaign => campaign.StatusCampaignId,
                    status => status.Id,
                    (campaign, status) => new { campaign, status })
                .SelectMany(x => x.status.DefaultIfEmpty(),
                    (x, status) => new { x.campaign, status })
                .GroupJoin(_context.Recipient,
                    combined => combined.campaign.RecipientId,
                    recipient => recipient.Id,
                    (combined, recipient) => new { combined.campaign, combined.status, recipient })
                .SelectMany(x => x.recipient.DefaultIfEmpty(),
                    (x, recipient) => new { x.campaign, x.status, recipient })
                .Where(x => x.campaign.OrganiserId == organiserId &&
                    (x.campaign.Id.ToString() == search.Campaign || (x.campaign.NormalizedTitle != null && x.campaign.NormalizedTitle.Contains(search.Campaign))) &&
                    (x.recipient.Id.ToString() == search.User || (x.recipient.NormalizedName != null && x.recipient.NormalizedName.Contains(search.User))) &&
                    ((search.StartDate == "" || x.campaign.StartDate.Value.Date >= DateTime.Parse(search.StartDate).Date) && (search.EndDate == "" || x.campaign.EndDate.Value.Date <= DateTime.Parse(search.EndDate).Date)) &&
                    (x.campaign.City != null && x.campaign.City.Contains(search.City)))
                .Skip((pageIndex - 1) * 20)
                .Take(20)
                .OrderByDescending(x => x.campaign.CreatedDate)
                .Select(x => new CampaignShortCDto
                {
                    Id = x.campaign.Id,
                    Title = x.campaign.Title,
                    Target = x.campaign.Target,
                    StartDate = x.campaign.StartDate == null ? "?" : x.campaign.StartDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    EndDate = x.campaign.EndDate == null ? "?" : x.campaign.EndDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Address = x.campaign.Address + ", " + x.campaign.City,
                    Status = x.status.Name,
                    UserId = x.recipient.Id,
                    UserName = x.recipient.Name,
                    UserAva = x.recipient.AvaSrc,
                    Received = x.campaign.Received == false ? "Chưa nhận" : "Đã nhận",
                    CreatedDate = x.campaign.CreatedDate == null ? "" : ((DateTime)x.campaign.CreatedDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Disabled = x.campaign.Disabled == false ? "Active" : "Disabled"
                })
                .ToListAsync();
            return campaigns;
        }
        public async Task<List<CampaignShortBDto>?> GetSearchedListByRecipient(int pageIndex, int recipientId, CampaignSearchADto search)
        {
            var campaigns = await _context.Campaign
                .GroupJoin(_context.StatusCampaign,
                    campaign => campaign.StatusCampaignId,
                    status => status.Id,
                    (campaign, status) => new { campaign, status })
                .SelectMany(x => x.status.DefaultIfEmpty(),
                    (x, status) => new { x.campaign, status })
                .GroupJoin(_context.Organiser,
                    combined => combined.campaign.OrganiserId,
                    organiser => organiser.Id,
                    (combined, organiser) => new { combined.campaign, combined.status, organiser })
                .SelectMany(x => x.organiser.DefaultIfEmpty(),
                    (x, organiser) => new { x.campaign, x.status, organiser })
                .GroupJoin(_context.Recipient,
                    combined => combined.campaign.RecipientId,
                    recipient => recipient.Id,
                    (combined, recipient) => new { combined.campaign, combined.status, combined.organiser, recipient })
                .SelectMany(x => x.recipient.DefaultIfEmpty(),
                    (x, recipient) => new { x.campaign, x.status, x.organiser, recipient })
                .Where(x => x.campaign.RecipientId == recipientId && x.campaign.Disabled == false && (x.campaign.Id.ToString() == search.Campaign || (x.campaign.NormalizedTitle != null && x.campaign.NormalizedTitle.Contains(search.Campaign))) &&
                    (x.organiser.Id.ToString() == search.User || (x.organiser.NormalizedName != null && x.organiser.NormalizedName.Contains(search.User))) &&
                    ((search.StartDate == "" || x.campaign.StartDate.Value.Date >= DateTime.Parse(search.StartDate).Date) && (search.EndDate == "" || x.campaign.EndDate.Value.Date <= DateTime.Parse(search.EndDate).Date)) &&
                    (x.campaign.City != null && x.campaign.City.Contains(search.City)))
                .Skip((pageIndex - 1) * 20)
                .Take(20)
                .OrderByDescending(x => x.campaign.CreatedDate)
                .Select(x => new CampaignShortBDto
                {
                    Id = x.campaign.Id,
                    Title = x.campaign.Title,
                    Target = x.campaign.Target,
                    StartDate = x.campaign.StartDate == null ? "?" : x.campaign.StartDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    EndDate = x.campaign.EndDate == null ? "?" : x.campaign.EndDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Address = x.campaign.Address + ", " + x.campaign.City,
                    CoverSrc = x.campaign.CoverSrc,
                    Status = x.status.Name,
                    OrganiserId = x.organiser.Id,
                    OrganiserName = x.organiser.Name,
                    OrganiserAva = x.organiser.AvaSrc,
                    RecipientId = x.recipient.Id,
                    RecipientName = x.recipient.Name,
                    Received = x.campaign.Received == false ? "Unreceived" : "Received"
                })
                .ToListAsync();
            return campaigns;
        }
        public async Task<CampaignDetailBDto?> GetById(int campaignId)
        {
            var campaigns = await _context.Campaign
                .GroupJoin(_context.StatusCampaign,
                    campaign => campaign.StatusCampaignId,
                    status => status.Id,
                    (campaign, status) => new { campaign, status })
                .SelectMany(x => x.status.DefaultIfEmpty(),
                    (x, status) => new { x.campaign, status })
                .GroupJoin(_context.Organiser,
                    combined => combined.campaign.OrganiserId,
                    organiser => organiser.Id,
                    (combined, organiser) => new { combined.campaign, combined.status, organiser })
                .SelectMany(x => x.organiser.DefaultIfEmpty(),
                    (x, organiser) => new { x.campaign, x.status, organiser })
                .GroupJoin(_context.Recipient,
                    combined => combined.campaign.RecipientId,
                    recipient => recipient.Id,
                    (combined, recipient) => new { combined.campaign, combined.status, combined.organiser, recipient })
                .SelectMany(x => x.recipient.DefaultIfEmpty(),
                    (x, recipient) => new { x.campaign, x.status, x.organiser, recipient })
                .Where(x => x.campaign.Id == campaignId)
                .Select(x => new CampaignDetailBDto
                {
                    Id = x.campaign.Id,
                    Title = x.campaign.Title,
                    Target = x.campaign.Target,
                    Description = x.campaign.Description,
                    StartDate = x.campaign.StartDate == null ? "?" : x.campaign.StartDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    EndDate = x.campaign.EndDate == null ? "?" : x.campaign.EndDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Address = x.campaign.Address + ", " + x.campaign.City,
                    TargetAmount = x.campaign.TargetAmount,
                    CoverSrc = x.campaign.CoverSrc,
                    Status = x.status.Name,
                    OrganiserId = x.organiser.Id,
                    OrganiserName = x.organiser.Name,
                    OrganiserAva = x.organiser.AvaSrc,
                    RecipientId = x.recipient.Id,
                    RecipientName = x.recipient.Name,
                    RecipientAva = x.recipient.AvaSrc,
                    Received = x.campaign.Received == false ? "Chưa nhận" : "Đã nhận",
                    RatedByRecipient = x.campaign.RatedByRecipient,
                    RatedContentByRecipient = x.campaign.RatedContentByRecipient
                })
                .FirstOrDefaultAsync();
            return campaigns;
        }
        public async Task<Campaign> Add(CampaignCUDto campaignCUDto, string? coverPublicId, string? coverSrc, int? organiserId)
        {
            string? normalizedText = StringExtension.NormalizeString(campaignCUDto.Title);
            var campaign = new Campaign()
            {
                Title = campaignCUDto.Title,
                NormalizedTitle = normalizedText,
                Target = campaignCUDto.Target,
                Description = campaignCUDto.Description,
                StartDate = DateTime.Parse(campaignCUDto.StartDate == null || campaignCUDto.StartDate == "" ? throw new Exception("Start date is required") : campaignCUDto.StartDate),
                EndDate = DateTime.Parse(campaignCUDto.EndDate == null || campaignCUDto.EndDate == "" ? throw new Exception("End date is required") : campaignCUDto.EndDate),
                Address = campaignCUDto.Address,
                City = campaignCUDto.City,
                StatusCampaignId = campaignCUDto.StatusCampaignId,
                TargetAmount = campaignCUDto.TargetAmount,
                CoverSrc = coverSrc,
                CoverSrcPublicId = coverPublicId,
                OrganiserId = organiserId,
                RecipientId = campaignCUDto.RecipientId,
                Received = false,
                RatedByRecipient = null,
                RatedContentByRecipient = null,
                CreatedDate = DateTime.Now,
                CreatedBy = organiserId,
                UpdatedDate = DateTime.Now,
                UpdatedBy = organiserId,
                Disabled = false

            };
            _context.Campaign.Add(campaign);
            await _context.SaveChangesAsync();
            return campaign;
        }
        public async Task<Campaign> Update(int campaignId, CampaignCUDto campaignCUDto, string? coverPublicId, string? coverSrc, int? organiserId)
        {
            var campaign = await _context.Campaign.Where(x => x.Id == campaignId).FirstOrDefaultAsync();
            if (campaign == null)
            {
                throw new Exception($"Not found campaign id {campaignId}");
            }

            if(campaign.OrganiserId != organiserId)
            {
                throw new Exception($"This campaign does not belong to you");
            }

            string? normalizedText = StringExtension.NormalizeString(campaignCUDto.Title);

            campaign.Title = campaignCUDto.Title;
            campaign.NormalizedTitle = normalizedText;
            campaign.Target = campaignCUDto.Target;
            campaign.Description = campaignCUDto.Description;
            campaign.StartDate = DateTime.Parse(campaignCUDto.StartDate == null || campaignCUDto.StartDate == "" ? throw new Exception("Start date is required") : campaignCUDto.StartDate);
            campaign.EndDate = DateTime.Parse(campaignCUDto.EndDate == null || campaignCUDto.EndDate == "" ? throw new Exception("End date is required") : campaignCUDto.EndDate);
            campaign.Address = campaignCUDto.Address;
            campaign.City = campaignCUDto.City;
            campaign.StatusCampaignId = campaignCUDto.StatusCampaignId;
            campaign.TargetAmount = campaignCUDto.TargetAmount;
            if(coverSrc != null)
            {
                campaign.CoverSrc = coverSrc;
                campaign.CoverSrcPublicId = coverPublicId;
            }
            campaign.OrganiserId = organiserId;
            campaign.RecipientId = campaignCUDto.RecipientId;
            campaign.UpdatedDate = DateTime.Now;
            campaign.UpdatedBy = organiserId;
            campaign.Disabled = campaignCUDto.Disabled == "0" ? false : true;


            _context.Campaign.Update(campaign);
            await _context.SaveChangesAsync();
            return campaign;
        }
        public async Task<bool> UpdateDisabledCampaign(int campaignId, bool disabled)
        {
            var campaign = await _context.Campaign.Where(x => x.Id == campaignId).FirstOrDefaultAsync();
            if (campaign == null)
            {
                throw new Exception($"Not found campaign id {campaignId}");
            }

            campaign.Disabled = disabled;

            _context.Campaign.Update(campaign);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateRecivedByRecipient(int campaignId, int recipientId, bool received)
        {
            var campaign = await _context.Campaign.Where(x => x.Id == campaignId && x.RecipientId == recipientId).FirstOrDefaultAsync();
            if (campaign == null)
            {
                throw new Exception($"Not found campaign id {campaignId}");
            }

            campaign.Received = received;

            _context.Campaign.Update(campaign);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateRatedByRecipient(int campaignId, int recipientId, RateCampaign rateCampaign)
        {
            var campaign = await _context.Campaign.Where(x => x.Id == campaignId && x.RecipientId == recipientId).FirstOrDefaultAsync();
            if (campaign == null)
            {
                throw new Exception($"Not found campaign id {campaignId}");
            }

            campaign.RatedByRecipient = rateCampaign.Rate;
            campaign.RatedContentByRecipient = rateCampaign.Comment;

            _context.Campaign.Update(campaign);
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<Campaign> Add(Campaign campaign)
        //{
        //    _context.Campaign.Add(campaign);
        //    await _context.SaveChangesAsync();
        //    return campaign;
        //}
        //public async Task<Campaign?> Get(int campaignId)
        //{
        //    var campaign = await _context.Campaign.Where(x => x.Id == campaignId).FirstOrDefaultAsync();
        //    return campaign;
        //}
        //public async Task<Campaign> Update(CampaignDto campaignDto)
        //{
        //    var campaign = await _context.Campaign.Where(x => x.Id == campaignDto.Id).FirstOrDefaultAsync();
        //    if (campaign == null)
        //    {
        //        throw new KeyNotFoundException("Campaign not found");
        //    }
        //    campaign.Title = campaignDto.Title;
        //    campaign.Target = campaignDto.Target;
        //    campaign.Description = campaignDto.Description;
        //    campaign.Address = campaignDto.Address;
        //    campaign.TargetAmount = campaignDto.TargetAmount;
        //    campaign.UpdatedDate = DateTime.Now;
        //    _context.Campaign.Update(campaign);
        //    await _context.SaveChangesAsync();
        //    return campaign;
        //}

        //public async Task<bool> Remove(int campaignId)
        //{
        //    var hasDonations = await (
        //        from cp in _context.Campaign
        //        join dt in _context.Donation
        //        on cp.Id equals dt.CampaignId
        //        where cp.Id == campaignId
        //        select dt.Id
        //        ).AnyAsync();
        //    if (hasDonations) return false;
        //    /*var campaign = await _context.Campaign.Where(x => x.Id == campaignId).FirstOrDefaultAsync();
        //    if (campaign == null)
        //    {
        //        return false;
        //    }
        //    var hasDonations = await _context.Donation.AnyAsync(d => d.CampaignId == campaignId);

        //    if (hasDonations)
        //    {
        //        //Check if Campaign existed in the Donation table
        //        return false;
        //    }*/
        //    var campaign = await _context.Campaign.Where(x => x.Id == campaignId).FirstOrDefaultAsync();
        //    if (campaign == null)
        //    {
        //        return false;
        //    }
        //    _context.Campaign.Remove(campaign);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<bool> ChangeStatus(int campaignId, int statusId)
        //{
        //    var campaign = await _context.Campaign.Where(x => x.Id == campaignId).FirstOrDefaultAsync();
        //    if (campaign == null)
        //    {
        //        return false;
        //    }
        //    //set status campaign Id to value of "Finished"
        //    campaign.StatusCampaign.Id = statusId;
        //    await _context.SaveChangesAsync();
        //    return true;
        //}
    }
}