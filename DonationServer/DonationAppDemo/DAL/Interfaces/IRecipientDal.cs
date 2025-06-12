using DonationAppDemo.DTOs;
using DonationAppDemo.Models;

namespace DonationAppDemo.DAL.Interfaces
{
    public interface IRecipientDal
    {
        Task<List<UserDto>> GetAll(int pageIndex);
        Task<List<UserDto>> GetSearchedList(int pageIndex, string text);
        Task<Recipient?> GetByPhoneNum(string phoneNum);
        Task<Recipient?> GetById(int id);
        Task<Recipient> Add(RecipientDto recipientDto);
    }
}
