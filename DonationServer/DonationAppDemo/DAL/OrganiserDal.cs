using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Helper;
using DonationAppDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DonationAppDemo.DAL
{
    public class OrganiserDal : IOrganiserDal
    {
        private readonly DonationDbContext _context;

        public OrganiserDal(DonationDbContext context)
        {
            _context = context;
        }
        public async Task<List<UserDto>> GetAll(int pageIndex)
        {
            var usersInformation = await _context.Organiser
                .Where(x => x.AcceptedBy != null)
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
            var usersInformation = await _context.Organiser
                .Where(x => x.AcceptedBy != null && (x.AccountId == normalizedText || x.Id.ToString() == normalizedText || (x.NormalizedName != null && EF.Functions.Like(x.NormalizedName, $"%{normalizedText}%"))))
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
        public async Task<List<Organiser>> GetAllUnCensored(int pageIndex)
        {
            var usersInformation = await _context.Organiser
                .Where(x => x.AcceptedBy == null)
                .Skip((pageIndex - 1) * 20)
                .Take(20)
                .ToListAsync();
            return usersInformation;
        }
        public async Task<List<Organiser>> GetSearchedUncensoredList(int pageIndex, string text)
        {
            string? normalizedText = StringExtension.NormalizeString(text);
            var usersInformation = await _context.Organiser
                .Where(x => x.AcceptedBy == null && (x.AccountId == normalizedText || x.Id.ToString() == normalizedText || (x.NormalizedName != null && EF.Functions.Like(x.NormalizedName, $"%{normalizedText}%"))))
                .Skip((pageIndex - 1) * 20)
                .Take(20)
                .ToListAsync();
            return usersInformation;
        }
        public async Task<Organiser?> GetById(int id)
        {
            var userInformation = await _context.Organiser.Where(x => x.Id == id).FirstOrDefaultAsync();
            return userInformation;
        }
        public async Task<Organiser?> GetByPhoneNum(string phoneNum)
        {
            var userInformation = await _context.Organiser.Where(x => x.AccountId == phoneNum).FirstOrDefaultAsync();
            return userInformation;
        }
        public async Task<Organiser> Add(OrganiserDto organiserDto, string? certificationPublicId)
        {
            string? normalizedText = StringExtension.NormalizeString(organiserDto.Name);
            var organiser = new Organiser()
            {
                Name = organiserDto.Name,
                NormalizedName = normalizedText,
                Dob = organiserDto.Dob,
                Email = organiserDto.Email,
                Address = organiserDto.Address,
                CertificationSrc = organiserDto.CertificationSrc,
                CertificationSrcPublicId = certificationPublicId,
                Description = organiserDto.Description,
                CreatedDate = DateTime.Now,
                CreatedBy = null,
                UpdatedDate = DateTime.Now,
                UpdatedBy = null,
                AcceptedBy = null,
                AcceptedDate = null,
                AccountId = organiserDto.PhoneNum,
            };
            _context.Organiser.Add(organiser);
            await _context.SaveChangesAsync();
            return organiser;
        }
        public async Task<Organiser> Update(int organiserId, OrganiserDto organiserDto)
        {
            var organiser = await _context.Organiser.Where(x => x.Id == organiserId).FirstOrDefaultAsync();
            if (organiser == null)
            {
                throw new Exception($"Not found user id {organiserId}");
            }

            string? normalizedText = StringExtension.NormalizeString(organiserDto.Name);

            organiser.Name = organiserDto.Name;
            organiser.NormalizedName = normalizedText;
            organiser.Dob = organiserDto.Dob;
            organiser.Email = organiserDto.Email;
            organiser.Address = organiserDto.Address;
            organiser.Description = organiserDto.Description;
            organiser.UpdatedDate = DateTime.Now;
            organiser.UpdatedBy = organiserId;


            _context.Organiser.Update(organiser);
            await _context.SaveChangesAsync();
            return organiser;
        }
        public async Task<Organiser> UpdateApprovement(int organiserId, int adminId)
        {
            var organiser = await _context.Organiser.Where(x => x.Id == organiserId).FirstOrDefaultAsync();
            if (organiser == null)
            {
                throw new Exception($"Not found user id {organiserId}");
            }

            organiser.AcceptedDate = DateTime.Now;
            organiser.AcceptedBy = adminId;

            _context.Organiser.Update(organiser);
            await _context.SaveChangesAsync();
            return organiser;
        }
        public async Task<Organiser> UpdateAva(int organiserId, string avaSrc, string avaSrcPublicId)
        {
            var organiser = await _context.Organiser.Where(x => x.Id == organiserId).FirstOrDefaultAsync();
            if (organiser == null)
            {
                throw new Exception($"Not found user id {organiserId}");
            }

            organiser.AvaSrc = avaSrc;
            organiser.AvaSrcPublicId = avaSrcPublicId;
            organiser.UpdatedDate = DateTime.Now;
            organiser.UpdatedBy = organiserId;


            _context.Organiser.Update(organiser);
            await _context.SaveChangesAsync();
            return organiser;
        }

        public async Task<Organiser> UpdateCertification(int organiserId, string certificationSrc, string certificationSrcPublicId)
        {
            var organiser = await _context.Organiser.Where(x => x.Id == organiserId).FirstOrDefaultAsync();
            if (organiser == null)
            {
                throw new Exception($"Not found user id {organiserId}");
            }

            organiser.CertificationSrc = certificationSrc;
            organiser.CertificationSrcPublicId = certificationSrcPublicId;
            organiser.UpdatedDate = DateTime.Now;
            organiser.UpdatedBy = organiserId;


            _context.Organiser.Update(organiser);
            await _context.SaveChangesAsync();
            return organiser;
        }

        public async Task<bool> Delete(int organiserId)
        {
            var organiser = await _context.Organiser.Where(x => x.Id == organiserId).FirstOrDefaultAsync();
            if (organiser == null)
            {
                throw new Exception($"Not found user id {organiserId}");
            }
            _context.Organiser.Remove(organiser);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
