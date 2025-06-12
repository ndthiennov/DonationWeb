using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using DonationAppDemo.Services.Interfaces;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

namespace DonationAppDemo.Services
{
    public class RateCampaignService : IRateCampaignService
    {
        private readonly IRateCampaignDal _rateCampaignDal;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RateCampaignService(IRateCampaignDal rateCampaignDal,
            IHttpContextAccessor httpContextAccessor)
        {
            _rateCampaignDal = rateCampaignDal;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<RateCampaignDto>?> GetListByCampaignId(int campaignId, int pageIndex)
        {
            var rates = await _rateCampaignDal.GetListByCampaignId(campaignId, pageIndex);
            return rates;
        }
        public async Task<RateCampaignDto?> Add(RateCampaign rateCampaign)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == "Id").Value.ToString();

            if(currentUserId != rateCampaign.DonorId.ToString())
            {
                return null;
            }

            rateCampaign.RatedDate = DateTime.Now;

            var result = await _rateCampaignDal.Add(rateCampaign);

            var rateCampaignResult = new RateCampaignDto
            {
                CampaignId = rateCampaign.CampaignId,
                DonorId = rateCampaign.DonorId,
                DonorName = null,
                DonorAva = null,
                Rate = rateCampaign.Rate,
                Content = rateCampaign.Comment,
                RatedDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
            };

            return rateCampaignResult;
        }
        public async Task<bool> RemoveByDonorId(int campaignId)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == "Id").Value.ToString();

            var result = await _rateCampaignDal.RemoveByDonorId(campaignId, Int32.Parse(currentUserId));
            return result;
        }
    }
}
