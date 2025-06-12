using DonationAppDemo.DTOs;
using System.Threading.Tasks;

namespace DonationAppDemo.Services.Interfaces
{
    public interface ITransferenceService
    {
        Task AddTransference(TransferenceDto transferenceDto);
        Task UpdateTransference(int id, TransferenceDto transferenceDto);
        Task DeleteTransference(int id);
        Task<object> GetAllTransferences(int campaignId, int page, int pageSize);
    }
}