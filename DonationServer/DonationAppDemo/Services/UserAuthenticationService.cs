using CloudinaryDotNet.Actions;
using DonationAppDemo.DAL;
using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using DonationAppDemo.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Twilio.Rest.Chat.V1.Service;

namespace DonationAppDemo.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IAccountDal _accountDal;
        private readonly IOrganiserDal _organiserDal;
        private readonly IDonorDal _donorDal;
        private readonly IAdminDal _adminDal;
        private readonly IRecipientDal _recipientDal;
        private readonly ITransactionDal _transactionDal;
        private readonly IUtilitiesService _utilitiesService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public UserAuthenticationService(IAccountDal accountDal,
            IOrganiserDal organiserDal,
            IDonorDal donorDal,
            IAdminDal adminDal,
            IRecipientDal recipientDal,
            ITransactionDal transactionDal,
            IUtilitiesService utilitiesService,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _accountDal = accountDal;
            _organiserDal = organiserDal;
            _donorDal = donorDal;
            _adminDal = adminDal;
            _recipientDal = recipientDal;
            _transactionDal = transactionDal;
            _utilitiesService = utilitiesService;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public async Task<string> CheckExistedUser(string phoneNum)
        {
            // Check existed account
            var user = await _accountDal.Get(phoneNum);
            if (user != null)
            {
                throw new Exception("TThis account is existed");
            }

            // Send otp for phone number verification
            var result = await _utilitiesService.TwilioSendCodeSms("+84" + phoneNum.Substring(1));
            return "Done";
        }
        public async Task<OrganiserDto> SignUpOrganiser(SignUpOrganiserDto signUpOrganiserDto)
        {
            // Verify otp
            var verified = await _utilitiesService.TwilioVerifyCodeSms(signUpOrganiserDto.Code, "+84" + signUpOrganiserDto.PhoneNum.Substring(1));
            if (verified == null || verified == false)
            {
                throw new Exception("Otp is not matched");
            }

            // Hash password
            var hashSaltResult = Helper.DataEncryptionExtensions.HMACSHA512(signUpOrganiserDto.Password);

            // Add certification image to cloudinary
            var uploadImageResult = await _utilitiesService.CloudinaryUploadPhotoAsync(signUpOrganiserDto.CertificationFile);
            if(uploadImageResult.Error != null)
            {
                throw new Exception("Cannot upload certidication image");
            }

            // Convert type data
            DateTime dob = DateTime.Parse(signUpOrganiserDto.Dob == null ? throw new Exception("Date of birth is required") : signUpOrganiserDto.Dob);

            // OrganiserDto
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

            // AccountDto
            var accountDto = new AccountDto()
            {
                PhoneNum = signUpOrganiserDto.PhoneNum,
                PasswordHash = hashSaltResult.hashedCode,
                PasswordSalt = hashSaltResult.keyCode,
                Role = "organiser",
                Disabled = false
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
        public async Task<DonorDto> SignUpDonor(SignUpDonorDto signUpDonorDto)
        {
            // Verify otp
            var verified = await _utilitiesService.TwilioVerifyCodeSms(signUpDonorDto.Code, "+84" + signUpDonorDto.PhoneNum.Substring(1));
            if (verified == null || verified == false)
            {
                throw new Exception("Otp is not matched");
            }

            // Hash password
            var hashSaltResult = Helper.DataEncryptionExtensions.HMACSHA512(signUpDonorDto.Password);

            // Convert type data
            DateTime dob = DateTime.Parse(signUpDonorDto.Dob == null ? throw new Exception("Date of birth is required") : signUpDonorDto.Dob);

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
                Disabled = false
            };

            // Add to account and organiser table in db
            var transactionResult = await _transactionDal.SignUpDonor(accountDto, donorDto);
            if (transactionResult == false)
            {
                throw new Exception("Sign up failed");
            }
            return donorDto;
        }
        public async Task<RecipientDto> SignUpRecipient(SignUpRecipientDto signUpRecipientDto)
        {
            // Verify otp
            var verified = await _utilitiesService.TwilioVerifyCodeSms(signUpRecipientDto.Code, "+84" + signUpRecipientDto.PhoneNum.Substring(1));
            if (verified == null || verified == false)
            {
                throw new Exception("Otp is not matched");
            }

            // Hash password
            var hashSaltResult = Helper.DataEncryptionExtensions.HMACSHA512(signUpRecipientDto.Password);

            // Convert type data
            DateTime dob = DateTime.Parse(signUpRecipientDto.Dob == null ? throw new Exception("Date of birth is required") : signUpRecipientDto.Dob);

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
                Disabled = false
            };

            // Add to account and organiser table in db
            var transactionResult = await _transactionDal.SignUpRecipient(accountDto, recipientDto);
            if (transactionResult == false)
            {
                throw new Exception("Sign up failed");
            }
            return recipientDto;
        }
        public async Task<string> SignIn(SignInDto signInDto)
        {
            // Check account condition
            var user = await _accountDal.Get(signInDto.PhoneNum);
            if (user == null)
            {
                throw new Exception("Account does not exist");
            }
            if (!user.Role.Contains(signInDto.Role))
            {
                throw new Exception("Account does not exist");
            }
            if (user.Disabled == true)
            {
                throw new Exception("Account has been locked or not approved");
            }
            if (!Helper.DataEncryptionExtensions.MatchCodeHashHMACSHA512(signInDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Password is not corrected");
            }

            // Get information of user and create claims
            var authClaims = new List<Claim>();
            if (signInDto.Role == "organiser")
            {
                var userInformation = await _organiserDal.GetByPhoneNum(user.PhoneNum);

                // Check approved organiser account
                if(userInformation == null)
                {
                    throw new Exception("Account is not existed");
                }

                if(userInformation.AcceptedBy == null)
                {
                    throw new Exception("Your account have not been approved yet");
                }

                // Create claims
                authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.PhoneNum),
                    new Claim(ClaimTypes.Name, userInformation.Name),
                    new Claim(type: "Id", value: userInformation.Id.ToString()),
                    new Claim(type: "AvaSrc", value: userInformation.AvaSrc != null ? userInformation.AvaSrc : ""),
                    new Claim(type: "CertificationSrc", value: userInformation.CertificationSrc),
                    new Claim(ClaimTypes.Role, signInDto.Role)
                };
            }
            else if (signInDto.Role == "donor")
            {
                var userInformation = await _donorDal.GetByPhoneNum(user.PhoneNum);

                // Create claims
                authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.PhoneNum),
                    new Claim(ClaimTypes.Name, userInformation.Name),
                    new Claim(type: "Id", value: userInformation.Id.ToString()),
                    new Claim(type: "AvaSrc", value: userInformation.AvaSrc != null ? userInformation.AvaSrc : ""),
                    new Claim(ClaimTypes.Role, signInDto.Role)
                };
            }
            else if (signInDto.Role == "admin")
            {
                var userInformation = await _adminDal.GetByPhoneNum(user.PhoneNum);

                // Create claims
                authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.PhoneNum),
                    new Claim(ClaimTypes.Name, userInformation.Name),
                    new Claim(type: "Id", value: userInformation.Id.ToString()),
                    new Claim(ClaimTypes.Role, signInDto.Role)
                };
            }
            else if (signInDto.Role == "recipient")
            {
                var userInformation = await _recipientDal.GetByPhoneNum(user.PhoneNum);

                // Create claims
                authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.PhoneNum),
                    new Claim(ClaimTypes.Name, userInformation.Name),
                    new Claim(type: "Id", value: userInformation.Id.ToString()),
                    new Claim(type: "AvaSrc", value: userInformation.AvaSrc != null ? userInformation.AvaSrc : "0"),
                    new Claim(ClaimTypes.Role, signInDto.Role)
                };
            }
            else
            {
                throw new Exception($"Undentified role {signInDto.Role}");
            }

            // Create Jwt
            var signInKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256Signature)
                );
            var tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenAsString;
        }
    }
}
