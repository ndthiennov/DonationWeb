using DonationAppDemo.DAL;
using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using DonationAppDemo.Services.Interfaces;

namespace DonationAppDemo.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminDal _adminDal;
        private readonly IUtilitiesService _utilitiesService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminService(IAdminDal adminDal,
            IUtilitiesService utilitiesService,
            IHttpContextAccessor httpContextAccessor)
        {
            _adminDal = adminDal;
            _utilitiesService = utilitiesService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<AdminDto>> GetAll(int pageIndex)
        {
            var admins = await _adminDal.GetAll(pageIndex);

            return admins;
        }
        public async Task<List<AdminDto>> GetSearchedList(int pageIndex, string text)
        {
            var admin = await _adminDal.GetSearchedList(pageIndex, text);

            return admin;
        }
        /*public async Task<Admin?> GetById(int adminId)
        {
            var admins = await _adminDal.GetById(adminId);

            return admins;
        }*/
        public async Task<Admin> Update(int adminId, AdminDto adminDto)
        {
            var admins = await _adminDal.Update(adminId, adminDto);

            return admins;
        }
    }
}
