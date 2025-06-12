using DonationAppDemo.Services.Interfaces;
using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using System.Threading.Tasks;
using System.Linq;

namespace DonationAppDemo.Services
{
    public class TransferenceService : ITransferenceService
    {
        private readonly ITransferenceDal _transferenceDal;
        private readonly ICampaignStatisticsDal _campaignStatisticsDal;

        public TransferenceService(ITransferenceDal transferenceDal, ICampaignStatisticsDal campaignStatisticsDal)
        {
            _transferenceDal = transferenceDal;
            _campaignStatisticsDal = campaignStatisticsDal;
        }

        public async Task AddTransference(TransferenceDto transferenceDto)
        {
            var transference = new Transference
            {
                Description = transferenceDto.Description,
                TransDate = transferenceDto.TransDate,
                Amount = transferenceDto.Amount,
                AdminId = transferenceDto.AdminId,
                CampaignId = transferenceDto.CampaignId
            };

            await _transferenceDal.AddAsync(transference);

            // Update Campaign Statistics
            await UpdateCampaignStatistics(transference.CampaignId);
        }

        private async Task UpdateCampaignStatistics(int? campaignId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTransference(int id, TransferenceDto transferenceDto)
        {
            var transference = await _transferenceDal.GetByIdAsync(id);
            if (transference == null)
            {
                throw new KeyNotFoundException("Transference not found");
            }

            transference.Description = transferenceDto.Description;
            transference.TransDate = transferenceDto.TransDate;
            transference.Amount = transferenceDto.Amount;
            transference.AdminId = transferenceDto.AdminId;

            await _transferenceDal.UpdateAsync(transference);

            // Update Campaign Statistics
            await UpdateCampaignStatistics(transference.CampaignId);
        }

        public async Task DeleteTransference(int id)
        {
            var transference = await _transferenceDal.GetByIdAsync(id);
            if (transference == null)
            {
                throw new KeyNotFoundException("Transference not found");
            }

            await _transferenceDal.DeleteAsync(transference);

            // Update Campaign Statistics
            await UpdateCampaignStatistics(transference.CampaignId);
        }

        public async Task<object> GetAllTransferences(int campaignId, int page, int pageSize)
        {
            var transferences = await _transferenceDal.GetByCampaignIdAsync(campaignId);
            var paginatedTransferences = transferences.Skip(page * pageSize).Take(pageSize).ToList();
            return new
            {
                transferences = paginatedTransferences,
                hasMore = transferences.Count() > (page + 1) * pageSize
            };
        }

        private async Task UpdateCampaignStatistics(int campaignId)
        {
            var totalAmount = (await _transferenceDal.GetByCampaignIdAsync(campaignId))
                                .Sum(t => t.Amount);

            var campaignStatistics = await _campaignStatisticsDal.GetByCampaignIdAsync(campaignId);
            if (campaignStatistics != null)
            {
                campaignStatistics.TotalTransferredAmount = totalAmount;
                await _campaignStatisticsDal.UpdateAsync(campaignStatistics);
            }
        }
    }
}