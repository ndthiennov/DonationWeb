using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using DonationAppDemo.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DonationAppDemo.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountDal _accountDal;
        private readonly ITransactionDal _transactionDal;
        private readonly IUtilitiesService _utilitiesService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(IAccountDal accountDal,
            ITransactionDal transactionDal,
            IUtilitiesService utilitiesService,
            IHttpContextAccessor httpContextAccessor)
        {
            _accountDal = accountDal;
            _transactionDal = transactionDal;
            _utilitiesService = utilitiesService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Account?> Get(string phoneNum)
        {
            var account = await _accountDal.Get(phoneNum);

            return account;
        }
        public async Task<bool> Delete(string phoneNum)
        {
            var result = await _accountDal.Delete(phoneNum);
            if (!result)
            {
                throw new Exception("Failed to delete account");
            }

            return result;
        }
        public async Task<bool> DeletePersonalAccount()
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value.ToString();

            var result = await _accountDal.Delete(currentUserId);
            if (!result)
            {
                throw new Exception("Failed to delete account");
            }

            return result;
        }
        public async Task<bool> UpdateDisabledAccount(string phoneNum, bool disabled)
        {
            var result = await _accountDal.UpdateDisabledAccount(phoneNum, disabled);
            if (!result)
            {
                throw new Exception("Failed to do disabled account");
            }

            return result;
        }

        public async Task<bool> UpdateDisabledPersonalAccount(bool disabled)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value.ToString();

            var result = await _accountDal.UpdateDisabledAccount(currentUserId, disabled);
            if (!result)
            {
                throw new Exception("Failed to do disabled account");
            }

            return result;
        }
        public async Task<OrganiserDto> AddOrganiserAccount(SignUpOrganiserDto signUpOrganiserDto)
        {
            // Check existed account
            var user = await _accountDal.Get(signUpOrganiserDto.PhoneNum);
            if (user != null)
            {
                throw new Exception("TThis account is existed");
            }

            // Hash password
            var hashSaltResult = Helper.DataEncryptionExtensions.HMACSHA512(signUpOrganiserDto.Password);

            //Add certification image to cloudinary
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

            // OrganiserDto
            var organiserDto = new OrganiserDto()
            {
                PhoneNum = signUpOrganiserDto.PhoneNum,
                Name = signUpOrganiserDto.Name,
                Dob = dob,
                Email = signUpOrganiserDto.Email,
                Address = signUpOrganiserDto.Address,
                CertificationSrc = uploadImageResult.SecureUrl.AbsoluteUri,
                Description = signUpOrganiserDto.Description,
            };

            // AccountDto
            var accountDto = new AccountDto()
            {
                PhoneNum = signUpOrganiserDto.PhoneNum,
                PasswordHash = hashSaltResult.hashedCode,
                PasswordSalt = hashSaltResult.keyCode,
                Role = "organiser",
                Disabled = disabled
            };

            // Add to account and organiser table in db
            var transactionResult = await _transactionDal.SignUpOrganiser(accountDto, organiserDto, uploadImageResult.PublicId);
            if (transactionResult == false)
            {
                await _utilitiesService.CloudinaryDeletePhotoAsync(uploadImageResult.PublicId);
                throw new Exception("Sign up failed");
            }
            return organiserDto;
        }

        public async Task<DonorDto> AddDonorAccount(SignUpDonorDto signUpDonorDto)
        {
            // Check existed account
            var user = await _accountDal.Get(signUpDonorDto.PhoneNum);
            if (user != null)
            {
                throw new Exception("TThis account is existed");
            }

            // Hash password
            var hashSaltResult = Helper.DataEncryptionExtensions.HMACSHA512(signUpDonorDto.Password);

            // Convert type data
            bool disabled = false;
            DateTime dob = DateTime.Parse(signUpDonorDto.Dob == null ? throw new Exception("Date of birth is required") : signUpDonorDto.Dob);

            if (signUpDonorDto.Disabled == "1")
            {
                disabled = true;
            }

            // DonorDto
            var donorDto = new DonorDto()
            {
                PhoneNum = signUpDonorDto.PhoneNum,
                Name = signUpDonorDto.Name,
                Gender = signUpDonorDto.Gender,
                Dob = dob,
                Email = signUpDonorDto.Email,
                Address = signUpDonorDto.Address
            };

            // AccountDto
            var accountDto = new AccountDto()
            {
                PhoneNum = signUpDonorDto.PhoneNum,
                PasswordHash = hashSaltResult.hashedCode,
                PasswordSalt = hashSaltResult.keyCode,
                Role = "donor",
                Disabled = disabled
            };

            // Add to account and organiser table in db
            var transactionResult = await _transactionDal.SignUpDonor(accountDto, donorDto);
            if (transactionResult == false)
            {
                throw new Exception("Sign up failed");
            }
            return donorDto;
        }
        public async Task<AdminDto> AddAdminAccount(SignUpAdminDto signUpAdminDto)
        {
            // Check existed account
            var user = await _accountDal.Get(signUpAdminDto.PhoneNum);
            if (user != null)
            {
                throw new Exception("TThis account is existed");
            }

            // Hash password
            var hashSaltResult = Helper.DataEncryptionExtensions.HMACSHA512(signUpAdminDto.Password);

            // Convert type data
            bool disabled = false;
            DateTime dob = DateTime.Parse(signUpAdminDto.Dob==null?throw new Exception("Date of birth is required") : signUpAdminDto.Dob);

            if (signUpAdminDto.Disabled == "1")
            {
                disabled = true;
            }

            // DonorDto
            var adminDto = new AdminDto()
            {
                PhoneNum = signUpAdminDto.PhoneNum,
                Name = signUpAdminDto.Name,
                Gender = signUpAdminDto.Gender,
                Dob = dob,
                Email = signUpAdminDto.Email
            };

            // AccountDto
            var accountDto = new AccountDto()
            {
                PhoneNum = signUpAdminDto.PhoneNum,
                PasswordHash = hashSaltResult.hashedCode,
                PasswordSalt = hashSaltResult.keyCode,
                Role = "admin",
                Disabled = disabled
            };

            // Add to account and organiser table in db
            var transactionResult = await _transactionDal.AccountAdmin(accountDto, adminDto);
            if (transactionResult == false)
            {
                throw new Exception("Sign up failed");
            }
            return adminDto;
        }
        public async Task<RecipientDto> AddRecipientAcccount(SignUpRecipientDto signUpRecipientDto)
        {
            // Check existed account
            var user = await _accountDal.Get(signUpRecipientDto.PhoneNum);
            if (user != null)
            {
                throw new Exception("TThis account is existed");
            }

            // Hash password
            var hashSaltResult = Helper.DataEncryptionExtensions.HMACSHA512(signUpRecipientDto.Password);

            // Convert type data
            bool disabled = false;
            DateTime dob = DateTime.Parse(signUpRecipientDto.Dob == null ? throw new Exception("Date of birth is required") : signUpRecipientDto.Dob);

            if (signUpRecipientDto.Disabled == "1")
            {
                disabled = true;
            }

            // DonorDto
            var recipientDto = new RecipientDto()
            {
                PhoneNum = signUpRecipientDto.PhoneNum,
                Name = signUpRecipientDto.Name,
                Gender = signUpRecipientDto.Gender,
                Dob = dob,
                Email = signUpRecipientDto.Email,
                Address = signUpRecipientDto.Address
            };

            // AccountDto
            var accountDto = new AccountDto()
            {
                PhoneNum = signUpRecipientDto.PhoneNum,
                PasswordHash = hashSaltResult.hashedCode,
                PasswordSalt = hashSaltResult.keyCode,
                Role = "recipient",
                Disabled = disabled
            };

            // Add to account and organiser table in db
            var transactionResult = await _transactionDal.SignUpRecipient(accountDto, recipientDto);
            if (transactionResult == false)
            {
                throw new Exception("Sign up failed");
            }
            return recipientDto;
        }

        public async Task<bool> DeleteUncensorOrganiserAccount(string phoneNum, int organiserId)
        {
            // Delete account (only have organiser role) and organiser from db
            var transactionResult = await _transactionDal.DeleteUncensoredOrganiser(phoneNum, organiserId);
            if (transactionResult == false)
            {
                throw new Exception("Sign up failed");
            }
            return true;
        }
    }
}
