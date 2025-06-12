using DonationAppDemo.DTOs;
using DonationAppDemo.Models;

namespace DonationAppDemo.DAL.Interfaces
{
    public interface IAdminDal
    {
        Task<List<AdminDto>> GetAll(int pageIndex);
        Task<List<AdminDto>> GetSearchedList(int pageIndex, string text);

        //Task<Admin?> GetById(int id);
        Task<Admin?> GetByPhoneNum(string phoneNum);
        Task<Admin> Add(AdminDto adminDto);
        Task<Admin> Update(int adminId, AdminDto adminDto);
    }
}
