using DonationAppDemo.Services.Interfaces;
using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using System.Threading.Tasks;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DonationAppDemo.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseDal _expenseDal;
        private readonly ICampaignParticipantService _participantService;
        private readonly ITransactionDal _transactionDal;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //private readonly ICampaignStatisticsDal _campaignStatisticsDal;

        public ExpenseService(IExpenseDal expenseDal,
            ICampaignParticipantService participantService,
            ITransactionDal transactionDal,
            INotificationService notificationService,
            IHttpContextAccessor httpContextAccessor
            /*, ICampaignStatisticsDal campaignStatisticsDal*/)
        {
            _expenseDal = expenseDal;
            _participantService = participantService;
            _transactionDal = transactionDal;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            //_campaignStatisticsDal = campaignStatisticsDal;
        }

        public async Task<List<Expense>?> GetListByCampaign(int campaignId)
        {
            var expense = await _expenseDal.GetListByCampaign(campaignId);
            return expense;
        }
        public async Task<Expense> Add(int campaignId, ExpenseDto expenseDto)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == "Id").Value.ToString();

            DateTime expenseDate = DateTime.Parse(expenseDto.ExpenseDate == null ? throw new Exception("Expense date is required") : expenseDto.ExpenseDate);

            var expense = new Expense
            {
                Description = expenseDto.Description,
                ExpenseDate = expenseDate,
                Amount = expenseDto.Amount,
                OrganiserId = Int32.Parse(currentUserId),
                CampaignId = campaignId
            };

            await _transactionDal.AddExpense(expense);

            // Notification
            List<int>? userIds = await _participantService.GetAllDonorIdByCampaignId(campaignId);
            var notification = new Notification
            {
                NotificationTitle = $"Chi tiêu chiến dịch {campaignId}",
                NotificationText = $"Chiến dịch {campaignId} đã chi {expense.Amount} vnd",
                NotificationDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                IsRead = false,
                Marked = false,
                FromUserId = Int32.Parse(currentUserId),
                FromUserRole = "organiser",
                ToUserId = null,
                ToUserRole = "donor"
            };

            var notiAddResult = await _notificationService.AddList(userIds, notification);
            var noti = await _notificationService.SendMultipleNotifications(userIds, "donor", notification.NotificationTitle, notification.NotificationText);
            return expense;
        }

        public async Task<Expense> Delete(int expenseId, int campaignId, ExpenseDto expenseDto)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == "Id").Value.ToString();

            DateTime expenseDate = DateTime.Parse(expenseDto.ExpenseDate == null ? throw new Exception("Expense date is required") : expenseDto.ExpenseDate);

            var expense = new Expense
            {
                Id = expenseId,
                Description = expenseDto.Description,
                ExpenseDate = expenseDate,
                Amount = expenseDto.Amount,
                OrganiserId = Int32.Parse(currentUserId),
                CampaignId = campaignId
            };

            await _transactionDal.DeleteExpense(expense);

            // Notification
            List<int>? userIds = await _participantService.GetAllDonorIdByCampaignId(campaignId);
            var notification = new Notification
            {
                NotificationTitle = $"Chi tiêu chiến dịch {campaignId}",
                NotificationText = $"Chiến dịch {campaignId} đã xóa chi tiêu {expense.Amount} vnd",
                NotificationDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                IsRead = false,
                Marked = false,
                FromUserId = Int32.Parse(currentUserId),
                FromUserRole = "organiser",
                ToUserId = null,
                ToUserRole = "donor"
            };

            var notiAddResult = await _notificationService.AddList(userIds, notification);
            var noti = await _notificationService.SendMultipleNotifications(userIds, "donor", notification.NotificationTitle, notification.NotificationText);
            
            return expense;
        }

        /*public async Task AddExpense(ExpenseDto expenseDto)
        {
            var expense = new Expense
            {
                Description = expenseDto.Description,
                ExpenseDate = expenseDto.ExpenseDate,
                Amount = expenseDto.Amount,
                OrganiserId = expenseDto.OrganiserId,
                CampaignId = expenseDto.CampaignId
            };

            await _expenseDal.AddAsync(expense);

            // Update Campaign Statistics
            await UpdateCampaignStatistics(expense.CampaignId);
        }

        public async Task UpdateExpense(int id, ExpenseDto expenseDto)
        {
            var expense = await _expenseDal.GetByIdAsync(id);
            if (expense == null)
            {
                throw new KeyNotFoundException("Expense not found");
            }

            expense.Description = expenseDto.Description;
            expense.ExpenseDate = expenseDto.ExpenseDate;
            expense.Amount = expenseDto.Amount;
            expense.OrganiserId = expenseDto.OrganiserId;

            await _expenseDal.UpdateAsync(expense);

            // Update Campaign Statistics
            await UpdateCampaignStatistics(expense.CampaignId);
        }

        public async Task DeleteExpense(int id)
        {
            var expense = await _expenseDal.GetByIdAsync(id);
            if (expense == null)
            {
                throw new KeyNotFoundException("Expense not found");
            }

            await _expenseDal.DeleteAsync(expense);

            // Update Campaign Statistics
            await UpdateCampaignStatistics(expense.CampaignId);
        }

        public async Task<object> GetAllExpenses(int campaignId, int page, int pageSize)
        {
            var expenses = await _expenseDal.GetByCampaignIdAsync(campaignId);
            var paginatedExpenses = expenses.Skip(page * pageSize).Take(pageSize).ToList();
            return new
            {
                expenses = paginatedExpenses,
                hasMore = expenses.Count() > (page + 1) * pageSize
            };
        }

        private async Task UpdateCampaignStatistics(int campaignId)
        {
            var totalAmount = (await _expenseDal.GetByCampaignIdAsync(campaignId))
                                .Sum(e => e.Amount);

            var campaignStatistics = await _campaignStatisticsDal.GetByCampaignIdAsync(campaignId);
            if (campaignStatistics != null)
            {
                campaignStatistics.TotalExpendedAmount = totalAmount;
                await _campaignStatisticsDal.UpdateAsync(campaignStatistics);
            }
        }*/
    }
}