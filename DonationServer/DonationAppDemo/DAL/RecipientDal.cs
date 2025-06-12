using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Helper;
using DonationAppDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DonationAppDemo.DAL
{
    public class RecipientDal : IRecipientDal
    {
        private readonly DonationDbContext _context;

        public RecipientDal(DonationDbContext context)
        {
            _context = context;
        }
        public async Task<List<UserDto>> GetAll(int pageIndex)
        {
            var usersInformation = await _context.Recipient
                .Skip((pageIndex - 1) * 20)
                .Take(20)
                .Join(_context.Account, user => user.AccountId, account => account.PhoneNum,
                    (user, account) => new UserDto()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        PhoneNum = account.PhoneNum,
                        Address = user.Address,
                        Disabled = account.Disabled == true ? "Disabled" : "Active"
                    })
                .ToListAsync();
            return usersInformation;
        }
        public async Task<List<UserDto>> GetSearchedList(int pageIndex, string text)
        {
            string? normalizedText = StringExtension.NormalizeString(text);
            var usersInformation = await _context.Recipient
                .Where(x => x.AccountId == normalizedText || x.Id.ToString() == normalizedText || (x.NormalizedName != null && EF.Functions.Like(x.NormalizedName, $"%{normalizedText}%")))
                .Skip((pageIndex - 1) * 20)
                .Take(20)
                .Join(_context.Account, user => user.AccountId, account => account.PhoneNum,
                    (user, account) => new UserDto()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        PhoneNum = account.PhoneNum,
                        Address = user.Address,
                        Disabled = account.Disabled == true ? "Disabled" : "Active"
                    })
                .ToListAsync();
            return usersInformation;
        }
        public async Task<Recipient?> GetByPhoneNum(string phoneNum)
        {
            var userInformation = await _context.Recipient.Where(x => x.AccountId == phoneNum).FirstOrDefaultAsync();
            return userInformation;
        }
        public async Task<Recipient?> GetById(int id)
        {
            var userInformation = await _context.Recipient.Where(x => x.Id == id).FirstOrDefaultAsync();
            return userInformation;
        }
        public async Task<Recipient> Add(RecipientDto recipientDto)
        {
            string? normalizedText = StringExtension.NormalizeString(recipientDto.Name);
            var recipient = new Recipient()
            {
                Name = recipientDto.Name,
                NormalizedName = normalizedText,
                Gender = recipientDto.Gender,
                Dob = recipientDto.Dob,
                Email = recipientDto.Email,
                Address = recipientDto.Address,
                CreatedDate = DateTime.Now,
                CreatedBy = null,
                UpdatedDate = DateTime.Now,
                UpdatedBy = null,
                AccountId = recipientDto.PhoneNum,
            };
            _context.Recipient.Add(recipient);
            await _context.SaveChangesAsync();
            return recipient;
        }
    }
}
