using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DonationAppDemo.DAL
{
    public class ExpenseDal : IExpenseDal
    {
        private readonly DonationDbContext _context;

        public ExpenseDal(DonationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Expense>?> GetListByCampaign(int campaignId)
        {
            var expenses = await _context.Expense.Where(x => x.CampaignId == campaignId).ToListAsync();
            return expenses;
        }
        public async Task<Expense> Add(Expense expense)
        {
            _context.Expense.Add(expense);
            await _context.SaveChangesAsync();

            return expense;
        }

        public async Task<Expense> Delete(int expenseId, int organiserId)
        {
            var expense = await _context.Expense.Where(x => x.Id == expenseId && x.OrganiserId == organiserId).FirstOrDefaultAsync();
            if (expense == null)
            {
                throw new Exception($"Did not find expense id {expenseId}");
            }

            _context.Expense.Remove(expense);
            await _context.SaveChangesAsync();

            return expense;
        }
        /*private readonly DonationDbContext _dbContext;

        public ExpenseDal(DonationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Expense expense)
        {
            await _dbContext.Expense.AddAsync(expense);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Expense expense)
        {
            _dbContext.Expense.Update(expense);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Expense expense)
        {
            _dbContext.Expense.Remove(expense);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Expense> GetByIdAsync(int id)
        {
            return await _dbContext.Expense.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Expense>> GetAllAsync()
        {
            return await _dbContext.Expense.ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetByCampaignIdAsync(int campaignId)
        {
            return await _dbContext.Expense.Where(e => e.CampaignId == campaignId).ToListAsync();
        }*/
    }
}