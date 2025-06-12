using DonationAppDemo.DTOs;
using DonationAppDemo.Models;

namespace DonationAppDemo.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Account?> Get(string phoneNum);
        Task<bool> Delete(string phoneNum); // admin
        Task<bool> DeletePersonalAccount(); // self-user
        Task<bool> UpdateDisabledAccount(string phoneNum, bool disabled); // admin
        Task<bool> UpdateDisabledPersonalAccount(bool disabled); // self-user
        Task<OrganiserDto> AddOrganiserAccount(SignUpOrganiserDto signUpOrganiserDto); // admin
        Task<DonorDto> AddDonorAccount(SignUpDonorDto signUpDonorDto); // admin
        Task<AdminDto> AddAdminAccount(SignUpAdminDto signUpAdminDto); // admin
        Task<RecipientDto> AddRecipientAcccount(SignUpRecipientDto signUpRecipientDto); // admin
        Task<bool> DeleteUncensorOrganiserAccount(string phoneNum, int organiserId); // admin
    }
}
