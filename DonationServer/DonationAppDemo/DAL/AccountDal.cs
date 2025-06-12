using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DonationAppDemo.DAL
{
    public class AccountDal : IAccountDal
    {
        private readonly DonationDbContext _context;

        public AccountDal(DonationDbContext context)
        {
            
            _context = context;
        }
        public async Task<Account?> Get(string phoneNum)
        {
            var user = await _context.Account.Where(x => x.PhoneNum == phoneNum && x.Disabled == false).FirstOrDefaultAsync();
            return user;
        }
        public async Task<Account> Add(AccountDto accountDto)
        {
            var account = new Account()
            {
                PhoneNum = accountDto.PhoneNum,
                PasswordHash = accountDto.PasswordHash,
                PasswordSalt = accountDto.PasswordSalt,
                Role = accountDto.Role,
                CreatedDate = DateTime.Now,
                CreatedBy = null,
                UpdatedDate = DateTime.Now,
                UpdatedBy = null,
                Disabled = accountDto.Disabled

            };
            _context.Account.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }
        public async Task<bool> UpdateDisabledAccount(string phoneNum, bool disabled)
        {
            var account = await _context.Account.Where(x => x.PhoneNum == phoneNum).FirstOrDefaultAsync();
            if (account == null)
            {
                throw new Exception($"Not found user's phone number {phoneNum}");
            }

            account.Disabled = disabled;

            _context.Account.Update(account);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateRole(string phoneNum, string role, bool disabled)
        {
            var account = await _context.Account.Where(x => x.PhoneNum == phoneNum).FirstOrDefaultAsync();
            if (account == null)
            {
                throw new Exception($"Not found user's phone number {phoneNum}");
            }
            else
            {
                if (account.Role.Contains(role))
                {
                    throw new Exception($"Role existed");
                }
            }

            account.Role = "," + account.Role + role;

            if (!account.Role.Contains("admin"))
            {
                account.Disabled = disabled;
            }
            
            _context.Account.Update(account);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Delete(string phoneNum)
        {
            var account = await _context.Account.Where(x => x.PhoneNum == phoneNum).FirstOrDefaultAsync();
            if (account == null)
            {
                throw new Exception($"Not found user's phone number {phoneNum}");
            }
            _context.Account.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteUncensoredOrganiser(string phoneNum)
        {
            var account = await _context.Account.Where(x => x.PhoneNum == phoneNum).FirstOrDefaultAsync();
            if (account == null)
            {
                throw new Exception($"Not found user's phone number {phoneNum}");
            }

            // Check role
            var roles = account.Role.Split(",").ToList();
            for(int i=0; i < roles.Count; i++)
            {
                if (roles[i].Equals("organiser"))
                {
                    roles.RemoveAt(i);
                    break;
                }
            }
            if(roles.Count == 0)
            {
                _context.Account.Remove(account);
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}
