using DonationAppDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DonationAppDemo.DAL.Interfaces
{
    public interface ITransferenceDal
    {
        Task AddAsync(Transference transference);
        Task UpdateAsync(Transference transference);
        Task DeleteAsync(Transference transference);
        Task<Transference> GetByIdAsync(int id);
        Task<IEnumerable<Transference>> GetByCampaignIdAsync(int campaignId);
    }
}