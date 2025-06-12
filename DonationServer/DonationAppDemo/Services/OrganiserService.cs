using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using DonationAppDemo.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Xml.Linq;

namespace DonationAppDemo.Services
{
    public class OrganiserService : IOrganiserService
    {
        private readonly IOrganiserDal _organiserDal;
        private readonly ITransactionDal _transactionDal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUtilitiesService _utilitiesService;

        public OrganiserService(IOrganiserDal organiserDal,
            ITransactionDal transactionDal,
            IHttpContextAccessor httpContextAccessor,
            IUtilitiesService utilitiesService)
        {
            _organiserDal = organiserDal;
            _transactionDal = transactionDal;
            _httpContextAccessor = httpContextAccessor;
            _utilitiesService = utilitiesService;
        }
        public async Task<List<UserDto>> GetAll(int pageIndex)
        {
            var organisers = await _organiserDal.GetAll(pageIndex);

            return organisers;
        }
        public async Task<List<UserDto>> GetSearchedList(int pageIndex, string text)
        {
            var organisers = await _organiserDal.GetSearchedList(pageIndex, text);

            return organisers;
        }
        public async Task<List<Organiser>> GetAllUnCensored(int pageIndex)
        {
            var organisers = await _organiserDal.GetAllUnCensored(pageIndex);

            return organisers;
        }
        public async Task<List<Organiser>> GetSearchedUncensoredList(int pageIndex, string text)
        {
            var organisers = await _organiserDal.GetSearchedUncensoredList(pageIndex, text);

            return organisers;
        }
        public async Task<Organiser?> GetById(int organiserId)
        {
            var organiser = await _organiserDal.GetById(organiserId);

            return organiser;
        }
        public async Task<Organiser> Update(int organiserId, OrganiserDto organiserDto)
        {
            var organiser = await _organiserDal.Update(organiserId, organiserDto);

            return organiser;
        }

        public async Task<Organiser> UpdateAva(int organiserId, IFormFile avaFile)
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
                var organiser = await _organiserDal.UpdateAva(organiserId, uploadImageResult.SecureUrl.AbsoluteUri, imagePublicId);

                return organiser;
            }
            catch
            {
                await _utilitiesService.CloudinaryDeletePhotoAsync(imagePublicId);
                throw new Exception("Error while updating on database");
            }
        }
        public async Task<Organiser> UpdateCertification(int organiserId, IFormFile certificationFile)
        {
            string imagePublicId = "";
            try
            {
                // Add avatar image to cloudinary
                var uploadImageResult = await _utilitiesService.CloudinaryUploadPhotoAsync(certificationFile);
                if (uploadImageResult.Error != null)
                {
                    throw new Exception("Cannot upload certification on cloudinary");
                }

                imagePublicId = uploadImageResult.PublicId;
                var organiser = await _organiserDal.UpdateCertification(organiserId, uploadImageResult.SecureUrl.AbsoluteUri, imagePublicId);

                return organiser;
            }
            catch
            {
                await _utilitiesService.CloudinaryDeletePhotoAsync(imagePublicId);
                throw new Exception("Error while updating on database");
            }
        }
        public async Task<bool> BecomeOrganiser(SignUpOrganiserDto signUpOrganiserDto)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var phoneNum = tokenS.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value.ToString();

            // Add certification image to cloudinary
            var uploadImageResult = await _utilitiesService.CloudinaryUploadPhotoAsync(signUpOrganiserDto.CertificationFile);
            if (uploadImageResult.Error != null)
            {
                throw new Exception("Cannot upload certidication image");
            }

            // Convert type data
            bool disabled = false;
            DateTime dob = DateTime.Parse(signUpOrganiserDto.Dob == null ? throw new Exception("Date of birth is required") : signUpOrganiserDto.Dob);

            if (signUpOrganiserDto.Disabled == "1")
            {
                disabled = true;
            }

            var organiserDto = new OrganiserDto()
            {
                PhoneNum = signUpOrganiserDto.PhoneNum,
                Name = signUpOrganiserDto.Name,
                Dob = dob,
                Email = signUpOrganiserDto.Email,
                Address = signUpOrganiserDto.Address,
                CertificationSrc = uploadImageResult.SecureUrl.AbsoluteUri,
                Description = signUpOrganiserDto.Description
            };

            // Check existed organiser role of account in AccountDal
            // Save to sql server db
            var transactionResult = await _transactionDal.BecomeOrganiser(phoneNum, "organiser", false, organiserDto, uploadImageResult.PublicId);
            if (transactionResult == false)
            {
                await _utilitiesService.CloudinaryDeletePhotoAsync(uploadImageResult.PublicId);
                throw new Exception("Sign up failed");
            }

            return true;
        }
        public async Task<Organiser> UpdateApprovement(int organiserId)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == "Id").Value.ToString();

            // Approve organiser
            var result = await _organiserDal.UpdateApprovement(organiserId, Int32.Parse(currentUserId));

            return result;
        }
    }
}
