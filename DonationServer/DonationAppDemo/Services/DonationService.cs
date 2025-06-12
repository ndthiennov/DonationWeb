using CloudinaryDotNet;
using DonationAppDemo.DAL;
using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Helper;
using DonationAppDemo.Models;
using DonationAppDemo.Services.Interfaces;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DonationAppDemo.Services
{
    public class DonationService : IDonationService
    {
        private readonly IDonationDal _donationDal;
        private readonly ITransactionDal _transactionDal;
        private readonly IUtilitiesService _utilitiesService;
        private readonly IDonorService _donorService;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DonationService(IDonationDal donationDal,
            ITransactionDal transactionDal,
            IUtilitiesService utilitiesService,
            IDonorService donorService,
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor)
        {
            _donationDal = donationDal;
            _transactionDal = transactionDal;
            _utilitiesService = utilitiesService;
            _donorService = donorService;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }
        /*public async Task<List<DonationDto>?> GetListByCampaignId(int campaignId, SearchDto searchDto)
        {
            // Get 20 records of donation
            var donations = await _donationDal.GetListByCampaignId(campaignId, searchDto.PageIndex, searchDto.FromDate, searchDto.ToDate, searchDto.Id == null ? null : Int32.Parse(searchDto.Id));

            if (donations == null)
            {
                return null;
            }

            // Get list of distict donor id according to donation list and get donor information list according to distict donor id list
            List<int?>? donorIds = new List<int?>();
            List<Donor>? donors = new List<Donor>();

            if(searchDto.Id == null)
            {
                donorIds = donations.Select(x => x.DonorId).Distinct().ToList();

                donors = await _donorService.GetByIdList(donorIds);
            }
            else
            {
                var donor = await _donorService.GetById(Int32.Parse(searchDto.Id));
                if (donor != null)
                {
                    donors.Add(donor);
                }
            }
            
            // Combine donation list and dornor information list by donor id
            List<DonationDto>? response = new List<DonationDto>();
            if (donors != null)
            {
                response = donations.SelectMany(donation => donors.Where(donor => donor.Id == donation.DonorId).Select(donor => new DonationDto
                {
                    DonorId = donation.DonorId,
                    DonorName = donor.Name,
                    DonorAvaSrc = donor.AvaSrc,
                    Amount = donation.Amount,
                    DonationDate = donation.DonationDate
                })).ToList();

                return response;
            }

            response = donations.Select(donation => new DonationDto
            {
                DonorId = donation.DonorId,
                DonorName = null,
                DonorAvaSrc = null,
                Amount = donation.Amount,
                DonationDate = donation.DonationDate
            }).ToList();

            return response;
        }

        public async Task<List<DonationDto>?> GetListByDonorId(SearchDto searchDto)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == "Id").Value.ToString();

            return await _donationDal.GetListByDonorId(Int32.Parse(currentUserId), searchDto.PageIndex, searchDto.FromDate, searchDto.ToDate);
        }*/
        public async Task<List<DonationDto>?> GetSearchedListByCampaignId(int campaignId, SearchDto searchDto)
        {
            // Convert type
            if (searchDto.FromDate != "" || searchDto.ToDate != "")
            {
                if (searchDto.ToDate == "" || searchDto.FromDate == "")
                {
                    throw new Exception("From date and To date can not be null if one of them is not null");
                }
            }
            else
            {
                //search.StartDate = DateTime.MinValue.ToString();
                //search.EndDate = DateTime.Now.ToString();

                searchDto.FromDate = "";
                searchDto.ToDate = "";
            }

            string? normalized = StringExtension.NormalizeString(searchDto.Donor);
            searchDto.Donor = normalized == null ? "" : normalized;

            // Do search
            var donations = await _donationDal.GetSearchedListByCampaignId(campaignId, searchDto);
            return donations;
        }
        public async Task<string> CreatePaymentUrl(HttpContext context, PaymentRequestDto request)
        {
            if (request.PaymentMethod == "vnpay")
            {
                return await _utilitiesService.VnPayCreatePaymentUrl(context, request);
            }
            else if (request.PaymentMethod == "zalopay")
            {
                return await _utilitiesService.ZaloPayCreatePaymentUrl(request);
            }
            else
            {
                throw new Exception($"{request.PaymentMethod} gateway is not supported");
            }
        }
        public async Task<DonationDto> PaymentExecute(IQueryCollection collections)
        {
            // Determine payment gateway
            string paymentMethod = "";
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.Equals("appid"))
                {
                    if(value == _config.GetValue<string>("ZaloPaySettings:AppId"))
                    {
                        paymentMethod = "zalopay";
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    paymentMethod = "vnpay";
                    break;
                }
            }

            PaymentResponseDto resultPayment = new PaymentResponseDto();
            if (paymentMethod == "vnpay")
            {
                resultPayment = await _utilitiesService.VnPayPaymentExecute(collections);
            }
            else if (paymentMethod == "zalopay")
            {
                resultPayment = await _utilitiesService.ZaloPayPaymentExecute(collections);
            }
            else
            {
                throw new Exception($"{paymentMethod} gateway is not supported");
            }

            // Check payment result
            if (resultPayment.PaymentResponse == false)
            {
                throw new Exception("Payment failed");
            }

            // Add to donation entity and update campaign statistics entity
            var transactionResult = await _transactionDal.AddDonation(resultPayment);
            if (transactionResult == null)
            {
                throw new Exception("Failed to update database");
            }

            // Get information of donor
            Donor? donor = await _donorService.GetById(resultPayment.UserId);
            if (donor == null)
            {
                throw new Exception($"Not found donor id {resultPayment.UserId}");
            }

            DonationDto donation = new DonationDto()
            {
                CampaignId = resultPayment.CampaignId,
                CampaignName = null,
                DonorId = resultPayment.UserId,
                DonorName = donor.Name,
                DonorAvaSrc = donor.AvaSrc,
                DonationDate = resultPayment.PaymentDate == null ? "?" : resultPayment.PaymentDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Amount = resultPayment.Amount,
                CampaignDonationTotal = transactionResult.TotalDonationAmount
            };

            return donation;
        }
    }
}
