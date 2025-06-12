using DonationAppDemo.DTOs;
using DonationAppDemo.Models;

namespace DonationAppDemo.DAL.Interfaces
{
    public interface IDonorDal
    {
        Task<List<UserDto>> GetAll(int pageIndex);
        Task<List<UserDto>> GetSearchedList(int pageIndex, string text);
        Task<Donor?> GetById(int id);
        Task<List<Donor>?> GetByIdList(List<int?>? donorIdList);
        Task<Donor?> GetByPhoneNum(string phoneNum);
        Task<Donor> Add(DonorDto donorDto);
        Task<Donor> Update(int donorId, DonorDto donorDto);
        Task<Donor> UpdateAva(int donorId, string avaSrc, string avaSrcPublicId);
    }
}
