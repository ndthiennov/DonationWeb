using DonationAppDemo.DTOs;
using DonationAppDemo.Models;

namespace DonationAppDemo.Services.Interfaces
{
    public interface IDonorService
    {
        Task<List<UserDto>> GetAll(int pageIndex);
        Task<List<UserDto>> GetSearchedList(int pageIndex, string text);
        Task<Donor?> GetById(int donorId);
        Task<List<Donor>?> GetByIdList(List<int?>? donorIdList);
        Task<Donor> Update(int donorId, DonorDto donorDto);
        Task<Donor> UpdateAva(int donorId, IFormFile avaFile);
        Task<bool> BecomeDonor(SignUpDonorDto signUpDonorDto);
    }
}
