using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Helper;
using DonationAppDemo.Models;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace DonationAppDemo.DAL
{
    public class AdminDal : IAdminDal
    {
        private readonly DonationDbContext _context;

        public AdminDal(DonationDbContext context)
        {
            _context = context;
        }
        public async Task<List<AdminDto>> GetAll(int pageIndex)
        {
            var usersInformation = await _context.Admin
                .Skip((pageIndex - 1) * 20)
                .Take(20)
                .Join(_context.Account, user => user.AccountId, account => account.PhoneNum,
                    (user, account) => new AdminDto()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Gender = user.Gender,
                        Dob = user.Dob,
                        Email = user.Email,
                        PhoneNum = account.PhoneNum,
                        Disabled = account.Disabled == true ? "Disabled" : "Active"
                    })
                .ToListAsync();
            return usersInformation;
        }
        public async Task<List<AdminDto>> GetSearchedList(int pageIndex, string text)
        {
            string? normalizedText = StringExtension.NormalizeString(text);
            var usersInformation = await _context.Admin
                .Where(x => x.AccountId == normalizedText || x.Id.ToString() == normalizedText || (x.NormalizedName != null && EF.Functions.Like(x.NormalizedName, $"%{normalizedText}%")))
                .Skip((pageIndex - 1) * 20)
                .Take(20)
                .Join(_context.Account, user => user.AccountId, account => account.PhoneNum,
                    (user, account) => new AdminDto()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Gender = user.Gender,
                        Dob = user.Dob,
                        Email = user.Email,
                        PhoneNum = account.PhoneNum,
                        Disabled = account.Disabled == true ? "Disabled" : "Active"
                    })
                .ToListAsync();
            return usersInformation;
        }
        /*public async Task<Admin?> GetById(int id)
        {
            var userInformation = await _context.Admin.Where(x => x.Id == id).FirstOrDefaultAsync();
            return userInformation;
        }*/
        public async Task<Admin?> GetByPhoneNum(string phoneNum)
        {
            var userInformation = await _context.Admin.Where(x => x.AccountId == phoneNum).FirstOrDefaultAsync();
            return userInformation;
        }
        public async Task<Admin> Add(AdminDto adminDto)
        {
            string? normalizedText = StringExtension.NormalizeString(adminDto.Name);
            var admin = new Admin()
            {
                Name = adminDto.Name,
                NormalizedName = normalizedText,
                Gender = adminDto.Gender,
                Dob = adminDto.Dob,
                Email = adminDto.Email,
                CreatedDate = DateTime.Now,
                CreatedBy = null,
                UpdatedDate = DateTime.Now,
                UpdatedBy = null,
                AccountId = adminDto.PhoneNum,
            };
            _context.Admin.Add(admin);
            await _context.SaveChangesAsync();
            return admin;
        }
        public async Task<Admin> Update(int adminId, AdminDto adminDto)
        {
            var admin = await _context.Admin.Where(x => x.Id == adminId).FirstOrDefaultAsync();
            if (admin == null)
            {
                throw new Exception($"Not found user id {adminId}");
            }

            string? normalizedText = StringExtension.NormalizeString(adminDto.Name);

            admin.Name = adminDto.Name;
            admin.NormalizedName = normalizedText;
            admin.Gender = adminDto.Gender;
            admin.Dob = adminDto.Dob;
            admin.Email = adminDto.Email;
            admin.UpdatedDate = DateTime.Now;
            admin.UpdatedBy = adminId;

            _context.Admin.Update(admin);
            await _context.SaveChangesAsync();
            return admin;
        }
    }
}
