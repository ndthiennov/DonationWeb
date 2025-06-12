using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace DonationAppDemo.DAL
{
    public class ImageCampaignDal : IImageCampaignDal
    {
        private readonly DonationDbContext _context;

        public ImageCampaignDal(DonationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ImageCampaignDto>?> GetAll(int pageIndex, int campaignId, int campaignStatusId)
        {
            var images = await _context.ImageCampaign
                .Where(x => x.CampaignId == campaignId && x.StatusCampaignId == campaignStatusId)
                .Skip((pageIndex - 1) * 8)
                .Take(8)
                .Select(x => new ImageCampaignDto
                {
                    Id = x.Id,
                    ImageSrc = x.ImageSrc
                })
                .ToListAsync();
            return images;
        }
        /*private readonly DonationDbContext _context;

        public ImageCampaignDal(DonationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ImageCampaign>> AddImages(List<ImageCampaign> imageCamapaigns)
        {
            foreach (var imageCampaign in imageCamapaigns)
            {
                _context.Add(imageCampaign);
            }
            await _context.SaveChangesAsync();
            return imageCamapaigns;
        }

        public async Task<bool> Remove(int imageId)
        {
            var image = await _context.ImageCampaign.Where(x => x.Id == imageId).FirstOrDefaultAsync();
            if (image == null)
            {
                return false;
            }
            _context.ImageCampaign.Remove(image);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveByCampaignId(int campaignId)
        {
            List<ImageCampaign> listImageCampaign = await _context.ImageCampaign.Where(x => x.CampaignId == campaignId).ToListAsync();
            if (listImageCampaign == null)
            {
                return false;
            }
            foreach (var image in listImageCampaign)
            {
                _context.ImageCampaign.Remove(image);
            }
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveListImages(List<ImageCampaignDto> imageCampaignDtos)
        {
            if (imageCampaignDtos == null)
            {
                return false;
            }
            foreach (var imageDto in imageCampaignDtos)
            {
                var image = await _context.ImageCampaign.Where(x => x.Id == imageDto.Id).FirstOrDefaultAsync();
                if (image == null)
                {
                    return false;
                }
                _context.ImageCampaign.Remove(image);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ImageCampaign>> GetById(int campaignId, int pageIndex)
        {
            // Fetch all image campaigns that match the given campaignId
            return await _context.ImageCampaign
                                    .Where(c => c.CampaignId == campaignId)
                                    .Skip((pageIndex - 1) * 10)
                                    .Take(10)
                                    .ToListAsync();
        }
        public async Task<List<ImageCampaign>> GetAllById(int campaignId)
        {
            // Fetch all image campaigns that match the given campaignId
            return await _context.ImageCampaign
                                    .Where(c => c.CampaignId == campaignId)
                                    .ToListAsync();
        }*/
    }
}