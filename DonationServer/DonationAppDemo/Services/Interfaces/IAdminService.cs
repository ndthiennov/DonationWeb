using DonationAppDemo.DTOs;
using DonationAppDemo.Models;

namespace DonationAppDemo.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<AdminDto>> GetAll(int pageIndex); // admin
        Task<List<AdminDto>> GetSearchedList(int pageIndex, string text);

        //Task<Admin?> GetById(int adminId); // admin
        Task<Admin> Update(int adminId, AdminDto adminDto); // admin
    }
}
