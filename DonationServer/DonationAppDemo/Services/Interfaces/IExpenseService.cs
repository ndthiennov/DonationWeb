using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using System.Threading.Tasks;

namespace DonationAppDemo.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<List<Expense>?> GetListByCampaign(int campaignId);
        Task<Expense> Add(int campaignId, ExpenseDto expenseDto);
        Task<Expense> Delete(int expenseId, int campaignId, ExpenseDto expenseDto);
        /*Task AddExpense(ExpenseDto expenseDto);
        Task UpdateExpense(int id, ExpenseDto expenseDto);
        Task DeleteExpense(int id);
        Task<object> GetAllExpenses(int campaignId, int page, int pageSize);*/
    }
}