using DonationAppDemo.DAL;
using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using DonationAppDemo.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DonationAppDemo.Services
{
    public class DonorService : IDonorService
    {
        private readonly IDonorDal _donorDal;
        private readonly ITransactionDal _transactionDal;
        private readonly IUtilitiesService _utilitiesService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DonorService(IDonorDal donorDal,
            ITransactionDal transactionDal,
            IUtilitiesService utilitiesService,
            IHttpContextAccessor httpContextAccessor)
        {
            _donorDal = donorDal;
            _transactionDal = transactionDal;
            _utilitiesService = utilitiesService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<UserDto>> GetAll(int pageIndex)
        {
            var donors = await _donorDal.GetAll(pageIndex);

            return donors;
        }
        public async Task<List<UserDto>> GetSearchedList(int pageIndex, string text)
        {
            var donor = await _donorDal.GetSearchedList(pageIndex, text);

            return donor;
        }
        public async Task<Donor?> GetById(int donorId)
        {
            var donor = await _donorDal.GetById(donorId);
            
            return donor;
        }
        public async Task<List<Donor>?> GetByIdList(List<int?>? donorIdList)
        {
            var donors = await _donorDal.GetByIdList(donorIdList);

            return donors;
        }
        public async Task<Donor> Update(int donorId, DonorDto donorDto)
        {
            var donor = await _donorDal.Update(donorId, donorDto);

            return donor;
        }
        public async Task<Donor> UpdateAva(int donorId, IFormFile avaFile)
        {
            string imagePublicId = "";
            try
            {
                // Add avatar image to cloudinary
                var uploadImageResult = await _utilitiesService.CloudinaryUploadPhotoAsync(avaFile);
                if (uploadImageResult.Error != null)
                {
                    throw new Exception("Cannot upload avatar on cloudinary");
                }

                imagePublicId = uploadImageResult.PublicId;
                var donor = await _donorDal.UpdateAva(donorId, uploadImageResult.SecureUrl.AbsoluteUri, imagePublicId);

                return donor;
            }
            catch
            {
                await _utilitiesService.CloudinaryDeletePhotoAsync(imagePublicId);
                throw new Exception("Error while updating on database");
            }
        }
        public async Task<bool> BecomeDonor(SignUpDonorDto signUpDonorDto)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var phoneNum = tokenS.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value.ToString();

            // Convert type data
            DateTime dob = DateTime.Parse(signUpDonorDto.Dob == null ? throw new Exception("Date of birth is required") : signUpDonorDto.Dob);

            var donorDto = new DonorDto()
            {
                Id = null,
                PhoneNum = phoneNum,
                Name = signUpDonorDto.Name,
                Gender = signUpDonorDto.Gender,
                Dob = dob,
                Email = signUpDonorDto.Email,
                Address = signUpDonorDto.Address,
                AvaSrc = null
            };

            // Check existed donor role of account in AccountDal
            // Save to sql server db
            return await _transactionDal.BecomeDonor(phoneNum, "donor", false, donorDto);
        }
    }
}
