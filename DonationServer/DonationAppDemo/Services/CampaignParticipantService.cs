using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DonationAppDemo.Services
{
    public class CampaignParticipantService : ICampaignParticipantService
    {
        private readonly ICampaignParticipantDal _campaignParticipantDal;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CampaignParticipantService(ICampaignParticipantDal campaignParticipantDal,
            IHttpContextAccessor httpContextAccessor)
        {
            _campaignParticipantDal = campaignParticipantDal;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<int>?> GetAllDonorIdByCampaignId(int campaignId)
        {
            var result = await _campaignParticipantDal.GetAllDonorIdByCampaignId(campaignId);
            return result;
        }
        public async Task<bool> CheckParticipated(int campaignId)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if(authHeader != null)
            {
                authHeader = authHeader.Replace("Bearer ", "");
                var jsonToken = handler.ReadToken(authHeader);
                var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
                var currentUserId = tokenS.Claims.First(claim => claim.Type == "Id").Value.ToString();

                var result = await _campaignParticipantDal.CheckParticipated(Int32.Parse(currentUserId), campaignId);

                return result;
            }
            
            return false;
        }
        public async Task<bool> JoinCampaign(int campaignId)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == "Id").Value.ToString();

            var result = await _campaignParticipantDal.JoinCampaign(campaignId, Int32.Parse(currentUserId));

            return result;
        }

        public async Task<bool> CancelCampaignPartipation(int campaignId)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == "Id").Value.ToString();

            var result = await _campaignParticipantDal.CancelCampaignPartipation(campaignId, Int32.Parse(currentUserId));

            return result;
        }
    }
}
