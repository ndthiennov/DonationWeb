using DonationAppDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DonationAppDemo.DAL.Interfaces
{
    public interface IExpenseDal
    {
        Task<List<Expense>?> GetListByCampaign(int campaignId);
        Task<Expense> Add(Expense expense);
        Task<Expense> Delete(int expenseId, int organiserId);
        /*Task AddAsync(Expense expense);
        Task UpdateAsync(Expense expense);
        Task DeleteAsync(Expense expense);
        Task<Expense> GetByIdAsync(int id);
        Task<IEnumerable<Expense>> GetAllAsync();
        Task<IEnumerable<Expense>> GetByCampaignIdAsync(int campaignId);*/
    }
}
