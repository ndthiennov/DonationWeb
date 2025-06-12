using CloudinaryDotNet.Actions;
using DonationAppDemo.DAL;
using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.DTOs;
using DonationAppDemo.Helper;
using DonationAppDemo.Models;
using DonationAppDemo.Services.Interfaces;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using static System.Net.Mime.MediaTypeNames;

namespace DonationAppDemo.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly string? _connectionString;
        private readonly IConfiguration _config;

        private readonly ICampaignDal _campaignDal;
        private readonly IRecipientService _recipientService;
        private readonly ICampaignParticipantService _participantService;
        private readonly INotificationService _notificationService;
        private readonly IUtilitiesService _utilitiesService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CampaignService(ICampaignDal campaignDal,
            IRecipientService recipientService,
            ICampaignParticipantService participantService,
            INotificationService notificationService,
            IUtilitiesService utilitiesService,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration config)
        {
            _campaignDal = campaignDal;
            _recipientService = recipientService;
            _participantService = participantService;
            _notificationService = notificationService;
            _utilitiesService = utilitiesService;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _connectionString = config.GetConnectionString("DonationDbConnection");
            
        }
        public async Task<List<CampaignShortADto>?> GetListByAdmin(int pageIndex)
        {
            var campaigns = await _campaignDal.GetListByAdmin(pageIndex);
            return campaigns;
        }
        public async Task<List<CampaignShortADto>?> GetSearchedListByAdmin(int pageIndex, CampaignSearchADto search)
        {
            // Convert type
            if(search.StartDate != "" || search.EndDate != "")
            {
                if (search.EndDate == "" || search.StartDate == "")
                {
                    throw new Exception("Start date and End date can not be null if one of them is not null");
                }
            }
            else
            {
                //search.StartDate = DateTime.MinValue.ToString();
                //search.EndDate = DateTime.Now.ToString();

                search.StartDate = "";
                search.EndDate = "";
            }

            string? normalized = StringExtension.NormalizeString(search.Campaign);
            search.Campaign = normalized == null ? "" : normalized;
            normalized = StringExtension.NormalizeString(search.User);
            search.User = normalized == null ? "" : normalized;

            // Do search
            var campaigns = await _campaignDal.GetSearchedListByAdmin(pageIndex, search);
            return campaigns;
        }
        public async Task<List<CampaignShortBDto>?> GetSearchedListByUser(int pageIndex, CampaignSearchADto search)
        {
            // Convert type
            if (search.StartDate != "" || search.EndDate != "")
            {
                if (search.EndDate == "" || search.StartDate == "")
                {
                    throw new Exception("Start date and End date can not be null if one of them is not null");
                }
            }
            else
            {
                //search.StartDate = DateTime.MinValue.ToString();
                //search.EndDate = DateTime.Now.ToString();

                search.StartDate = "";
                search.EndDate = "";
            }

            string? normalized = StringExtension.NormalizeString(search.Campaign);
            search.Campaign = normalized == null ? "" : normalized;
            normalized = StringExtension.NormalizeString(search.User);
            search.User = normalized == null ? "" : normalized;

            // Do search
            var campaigns = await _campaignDal.GetSearchedListByUser(pageIndex, search);
            return campaigns;
        }

        public async Task<List<CampaignShortCDto>?> GetSearchedListByOrganiser(int pageIndex, CampaignSearchADto search)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == "Id").Value.ToString();

            // Convert type
            if (search.StartDate != "" || search.EndDate != "")
            {
                if (search.EndDate == "" || search.StartDate == "")
                {
                    throw new Exception("Start date and End date can not be null if one of them is not null");
                }
            }
            else
            {
                //search.StartDate = DateTime.MinValue.ToString();
                //search.EndDate = DateTime.Now.ToString();

                search.StartDate = "";
                search.EndDate = "";
            }

            string? normalized = StringExtension.NormalizeString(search.Campaign);
            search.Campaign = normalized == null ? "" : normalized;
            normalized = StringExtension.NormalizeString(search.User);
            search.User = normalized == null ? "" : normalized;

            // Do search
            var campaigns = await _campaignDal.GetSearchedListByOrganiser(pageIndex, search, Int32.Parse(currentUserId));
            return campaigns;
        }
        public async Task<List<CampaignShortBDto>?> GetSearchedListByRecipient(int pageIndex, CampaignSearchADto search)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == "Id").Value.ToString();

            // Convert type
            if (search.StartDate != "" || search.EndDate != "")
            {
                if (search.EndDate == "" || search.StartDate == "")
                {
                    throw new Exception("Start date and End date can not be null if one of them is not null");
                }
            }
            else
            {
                //search.StartDate = DateTime.MinValue.ToString();
                //search.EndDate = DateTime.Now.ToString();

                search.StartDate = "";
                search.EndDate = "";
            }

            string? normalized = StringExtension.NormalizeString(search.Campaign);
            search.Campaign = normalized == null ? "" : normalized;
            normalized = StringExtension.NormalizeString(search.User);
            search.User = normalized == null ? "" : normalized;

            // Do search
            var campaigns = await _campaignDal.GetSearchedListByRecipient(pageIndex, Int32.Parse(currentUserId), search);
            return campaigns;
        }
        public async Task<CampaignDetailBDto?> GetById(int campaignId)
        {
            var campaign = await _campaignDal.GetById(campaignId);
            return campaign;
        }
        public async Task<CampaignShortCDto> Add(CampaignCUDto campaignCUDto)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == "Id").Value.ToString();

            // Add cover image to cloudinary
            ImageUploadResult? coverImageResult = null;
            if (campaignCUDto.CoverSrc != null)
            {
                coverImageResult = await _utilitiesService.CloudinaryUploadPhotoAsync(campaignCUDto.CoverSrc);
                if (coverImageResult.Error != null)
                {
                    throw new Exception("Cannot upload certidication image");
                }
            }

            // Add campaign to db
            var campaign = await _campaignDal.Add(campaignCUDto, coverImageResult == null ? null : coverImageResult.PublicId, coverImageResult == null ? null : coverImageResult.SecureUrl.AbsoluteUri, Int32.Parse(currentUserId));

            var recipient = await _recipientService.GetById((int)campaignCUDto.RecipientId);

            var campaignDto = new CampaignShortCDto
            {
                Id = campaign.Id,
                Title = campaign.Title,
                Target = campaign.Target,
                StartDate = campaign.StartDate == null ? "?" : campaign.StartDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                EndDate = campaign.EndDate == null ? "?" : campaign.EndDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Address = campaign.Address + ", " + campaign.City,
                Status = campaign.StatusCampaignId == 1 ? "Đang chuẩn bị" : campaign.StatusCampaignId == 2 ? "Đang tiến hành" : campaign.StatusCampaignId == 3 ? "Đã kết thúc" : null,
                UserId = recipient.Id,
                UserName = recipient.Name,
                UserAva = recipient.AvaSrc,
                Received = campaign.Received == false ? "Chưa nhận" : "Đã nhận",
                CreatedDate = campaign.CreatedDate == null ? "" : ((DateTime)campaign.CreatedDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Disabled = campaign.Disabled == false ? "Active" : "Disabled"
            };
            return campaignDto;
        }
        public async Task<CampaignShortCDto> Update(int campaignId, CampaignCUDto campaignCUDto)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == "Id").Value.ToString();

            // Add cover image to cloudinary
            ImageUploadResult? coverImageResult = null;
            if (campaignCUDto.CoverSrc != null)
            {
                coverImageResult = await _utilitiesService.CloudinaryUploadPhotoAsync(campaignCUDto.CoverSrc);
                if (coverImageResult.Error != null)
                {
                    throw new Exception("Cannot upload certidication image");
                }
            }

            // Update campaign to db
            var campaign = await _campaignDal.Update(campaignId, campaignCUDto, coverImageResult == null ? null : coverImageResult.PublicId, coverImageResult == null ? null : coverImageResult.SecureUrl.AbsoluteUri, Int32.Parse(currentUserId));

            var recipient = await _recipientService.GetById((int)campaignCUDto.RecipientId);

            var campaignDto = new CampaignShortCDto
            {
                Id = campaign.Id,
                Title = campaign.Title,
                Target = campaign.Target,
                StartDate = campaign.StartDate == null ? "?" : campaign.StartDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                EndDate = campaign.EndDate == null ? "?" : campaign.EndDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Address = campaign.Address + ", " + campaign.City,
                Status = campaign.StatusCampaignId == 1 ? "Đang chuẩn bị" : campaign.StatusCampaignId == 2 ? "Đang tiến hành" : campaign.StatusCampaignId == 3 ? "Đã kết thúc" : null,
                UserId = recipient.Id,
                UserName = recipient.Name,
                UserAva = recipient.AvaSrc,
                Received = campaign.Received == false ? "Chưa nhận" : "Đã nhận",
                CreatedDate = campaign.CreatedDate == null ? "" : ((DateTime)campaign.CreatedDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Disabled = campaign.Disabled == false ? "Active" : "Disabled"
            };

            // Notification
            List<int>? userIds = await _participantService.GetAllDonorIdByCampaignId(campaignId);
            var notification = new Notification
            {
                NotificationTitle = $"Cập nhật chiến dịch {campaign.Title}",
                NotificationText = $"Cập nhật thông tin chiến dịch {campaign.Title}",
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

            return campaignDto;
        }
        public async Task<bool> UpdateDisabledCampaign(int campaignId, bool disabled)
        {
            var result = await _campaignDal.UpdateDisabledCampaign(campaignId, disabled);
            if (!result)
            {
                throw new Exception("Failed to update disabled campaign");
            }

            return result;
        }

        public async Task<bool> UpdateRecivedByRecipient(int campaignId, bool received)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == "Id").Value.ToString();

            var result = await _campaignDal.UpdateRecivedByRecipient(campaignId, Int32.Parse(currentUserId), received);
            if (!result)
            {
                throw new Exception("Failed to update received campaign by recipient");
            }

            return result;
        }

        public async Task<bool> UpdateRatedByRecipient(int campaignId, RateCampaign rateCampaign)
        {
            // Get current user
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadJwtToken(authHeader) as JwtSecurityToken;
            var currentUserId = tokenS.Claims.First(claim => claim.Type == "Id").Value.ToString();

            var result = await _campaignDal.UpdateRatedByRecipient(campaignId, Int32.Parse(currentUserId), rateCampaign);
            if (!result)
            {
                throw new Exception("Failed to update received campaign by recipient");
            }

            return result;
        }

        //Export file Excell
        public MemoryStream GenerateExcelReportDonations(int campaignId, DateTime startDate, DateTime endDate)
        {
            var donationsByCampaign = new List<DonationDetail>();
            var allDonationsByDate = new List<DonationDetail>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Lấy danh sách đóng góp theo CampaignId
                var queryByCampaign = @"
                SELECT Id, DonationDate, Amount, DonorId, CampaignId, PaymentDescription
                FROM Donation
                WHERE CampaignId = @CampaignId
                AND DonationDate >= @StartDate 
                AND DonationDate <= @EndDate";

                using (var command = new SqlCommand(queryByCampaign, connection))
                {
                    command.Parameters.AddWithValue("@CampaignId", campaignId);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            donationsByCampaign.Add(new DonationDetail
                            {
                                Id = reader.GetInt32(0),
                                DonationDate = reader.GetDateTime(1),
                                Amount = reader.IsDBNull(2) ? 0 : Convert.ToDouble(reader.GetDecimal(2)),
                                DonorId = reader.GetInt32(3),
                                CampaignId = reader.GetInt32(4),
                                PaymentDescription = reader.IsDBNull(5) ? null : reader.GetString(5),
                            });
                        }
                    }
                }

                // Lấy tất cả các khoản đóng góp không theo CampaignId nhưng theo ngày
                var queryAllDonations = @"
                SELECT Id, DonationDate, Amount, DonorId, CampaignId, PaymentDescription
                FROM Donation
                WHERE DonationDate >= @StartDate 
                AND DonationDate <= @EndDate";

                using (var command = new SqlCommand(queryAllDonations, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allDonationsByDate.Add(new DonationDetail
                            {
                                Id = reader.GetInt32(0),
                                DonationDate = reader.GetDateTime(1),
                                Amount = reader.IsDBNull(2) ? 0 : Convert.ToDouble(reader.GetDecimal(2)),
                                DonorId = reader.GetInt32(3),
                                CampaignId = reader.GetInt32(4),
                                PaymentDescription = reader.IsDBNull(5) ? null : reader.GetString(5),
                            });
                        }
                    }
                }
            }

            // Tạo file Excel từ dữ liệu
            using (var package = new ExcelPackage())
            {
                // Sheet 1: Donations theo CampaignId
                var worksheetByCampaign = package.Workbook.Worksheets.Add("Donations By Campaign");
                worksheetByCampaign.Cells[1, 1].Value = "STT";
                worksheetByCampaign.Cells[1, 2].Value = "Donation Date";
                worksheetByCampaign.Cells[1, 3].Value = "Amount";
                worksheetByCampaign.Cells[1, 4].Value = "Donor ID";
                worksheetByCampaign.Cells[1, 5].Value = "Campaign Id";
                worksheetByCampaign.Cells[1, 6].Value = "Payment Description";

                int row = 2;
                int stt = 1;
                foreach (var donation in donationsByCampaign)
                {
                    worksheetByCampaign.Cells[row, 1].Value = stt;
                    worksheetByCampaign.Cells[row, 2].Value = donation.DonationDate.ToString("yyyy-MM-dd");
                    worksheetByCampaign.Cells[row, 2].Style.Numberformat.Format = "dd-mm-yy";

                    worksheetByCampaign.Cells[row, 3].Value = donation.Amount;
                    worksheetByCampaign.Cells[row, 4].Value = donation.DonorId;
                    worksheetByCampaign.Cells[row, 5].Value = donation.CampaignId;
                    worksheetByCampaign.Cells[row, 6].Value = donation.PaymentDescription;
                    row++;
                    stt++;
                }

                // Tính tổng số tiền cho sheet 1
                worksheetByCampaign.Cells[row, 2].Value = "Total Amount:";
                worksheetByCampaign.Cells[row, 2].Style.Font.Bold = true;
                worksheetByCampaign.Cells[row, 3].Formula = $"SUM(C2:C{row - 1})"; // Công thức SUM cho cột Amount
                worksheetByCampaign.Cells[row, 3].Style.Font.Bold = true;

                // Sheet 2: Tất cả Donations theo ngày
                var worksheetAllDonations = package.Workbook.Worksheets.Add("All Donations By Date");
                worksheetAllDonations.Cells[1, 1].Value = "STT";
                worksheetAllDonations.Cells[1, 2].Value = "Donation Date";
                worksheetAllDonations.Cells[1, 3].Value = "Amount";
                worksheetAllDonations.Cells[1, 4].Value = "Donor ID";
                worksheetAllDonations.Cells[1, 5].Value = "Campaign Id";
                worksheetAllDonations.Cells[1, 6].Value = "Payment Description";

                row = 2;
                stt = 1;
                foreach (var donation in allDonationsByDate)
                {
                    worksheetAllDonations.Cells[row, 1].Value = stt;
                    worksheetAllDonations.Cells[row, 2].Value = donation.DonationDate.ToString("yyyy-MM-dd");
                    worksheetAllDonations.Cells[row, 2].Style.Numberformat.Format = "dd-mm-yy";

                    worksheetAllDonations.Cells[row, 3].Value = donation.Amount;
                    worksheetAllDonations.Cells[row, 4].Value = donation.DonorId;
                    worksheetAllDonations.Cells[row, 5].Value = donation.CampaignId;
                    worksheetAllDonations.Cells[row, 6].Value = donation.PaymentDescription;
                    row++;
                    stt++;
                }

                // Tính tổng số tiền cho sheet 2
                worksheetAllDonations.Cells[row, 2].Value = "Total Amount:";
                worksheetAllDonations.Cells[row, 2].Style.Font.Bold = true;
                worksheetAllDonations.Cells[row, 3].Formula = $"SUM(C2:C{row - 1})"; // Công thức SUM cho cột Amount
                worksheetAllDonations.Cells[row, 3].Style.Font.Bold = true;

                // Lưu vào MemoryStream và trả về
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                return stream;
            }
        }

        //public MemoryStream GenerateExcelReportDonations(int campaignId, DateTime startDate, DateTime endDate)
        //{
        //    var donations = new List<DonationDetail>();

        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();
        //        var query = @"
        //    SELECT Id, DonationDate, Amount, DonorId, CampaignId, PaymentDescription
        //    FROM Donation
        //    WHERE CampaignId = @CampaignId
        //      AND DonationDate >= @StartDate 
        //      AND DonationDate <= @EndDate";

        //        using (var command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@CampaignId", campaignId);
        //            command.Parameters.AddWithValue("@StartDate", startDate);
        //            command.Parameters.AddWithValue("@EndDate", endDate);

        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    donations.Add(new DonationDetail
        //                    {
        //                        Id = reader.GetInt32(0),
        //                        DonationDate = reader.GetDateTime(1),
        //                        Amount = reader.IsDBNull(2) ? 0 : Convert.ToDouble(reader.GetDecimal(2)),
        //                        DonorId = reader.GetInt32(3),
        //                        CampaignId = reader.GetInt32(4),
        //                        PaymentDescription = reader.IsDBNull(5) ? null : reader.GetString(5),
        //                    });
        //                }
        //            }
        //        }
        //    }

        //    // Tạo file Excel từ dữ liệu
        //    using (var package = new ExcelPackage())
        //    {
        //        var worksheet = package.Workbook.Worksheets.Add("Donations");
        //        worksheet.Cells[1, 1].Value = "STT";
        //        worksheet.Cells[1, 2].Value = "Donation Date";
        //        worksheet.Cells[1, 3].Value = "Amount";
        //        worksheet.Cells[1, 4].Value = "Donor ID";
        //        worksheet.Cells[1, 5].Value = "Campaign Id";
        //        worksheet.Cells[1, 6].Value = "Payment Description";

        //        int row = 2;
        //        int stt = 1;
        //        foreach (var donation in donations)
        //        {
        //            worksheet.Cells[row, 1].Value = stt;
        //            worksheet.Cells[row, 2].Value = donation.DonationDate.ToString("yyyy-MM-dd");
        //            worksheet.Cells[row, 2].Style.Numberformat.Format = "dd-mm-yy"; // Định dạng ngày tháng

        //            worksheet.Cells[row, 3].Value = donation.Amount;
        //            worksheet.Cells[row, 4].Value = donation.DonorId;
        //            worksheet.Cells[row, 5].Value = donation.CampaignId;
        //            worksheet.Cells[row, 6].Value = donation.PaymentDescription;
        //            row++;
        //            stt++;
        //        }

        //        var stream = new MemoryStream();
        //        package.SaveAs(stream);
        //        stream.Position = 0;
        //        return stream;
        //    }
        //}
        public MemoryStream GenerateExcelReportExpense(int campaignId, DateTime startDate, DateTime endDate)
        {
            var campaignExpenses = new List<ExpenseDetail>();
            var allExpenses = new List<ExpenseDetail>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Query 1: Lấy dữ liệu chi tiêu theo CampaignId
                var campaignQuery = @"
        SELECT Id, Description, ExpenseDate, Amount, OrganiserId, CampaignId
        FROM Expense
        WHERE CampaignId = @CampaignId
          AND ExpenseDate >= @StartDate 
          AND ExpenseDate <= @EndDate";

                using (var campaignCommand = new SqlCommand(campaignQuery, connection))
                {
                    campaignCommand.Parameters.AddWithValue("@CampaignId", campaignId);
                    campaignCommand.Parameters.AddWithValue("@StartDate", startDate);
                    campaignCommand.Parameters.AddWithValue("@EndDate", endDate);

                    using (var reader = campaignCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            campaignExpenses.Add(new ExpenseDetail
                            {
                                Id = reader.GetInt32(0),
                                Description = reader.IsDBNull(1) ? null : reader.GetString(1),
                                ExpenseDate = reader.GetDateTime(2),
                                Amount = reader.IsDBNull(3) ? 0 : Convert.ToDouble(reader.GetDecimal(3)),
                                OrganiserId = reader.GetInt32(4),
                                CampaignId = reader.GetInt32(5),
                            });
                        }
                    }
                }

                // Query 2: Lấy toàn bộ danh sách chi tiêu theo ngày (không theo CampaignId)
                var allQuery = @"
        SELECT Id, Description, ExpenseDate, Amount, OrganiserId, CampaignId
        FROM Expense
        WHERE ExpenseDate >= @StartDate 
          AND ExpenseDate <= @EndDate";

                using (var allCommand = new SqlCommand(allQuery, connection))
                {
                    allCommand.Parameters.AddWithValue("@StartDate", startDate);
                    allCommand.Parameters.AddWithValue("@EndDate", endDate);

                    using (var reader = allCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allExpenses.Add(new ExpenseDetail
                            {
                                Id = reader.GetInt32(0),
                                Description = reader.IsDBNull(1) ? null : reader.GetString(1),
                                ExpenseDate = reader.GetDateTime(2),
                                Amount = reader.IsDBNull(3) ? 0 : Convert.ToDouble(reader.GetDecimal(3)),
                                OrganiserId = reader.GetInt32(4),
                                CampaignId = reader.GetInt32(5),
                            });
                        }
                    }
                }
            }

            // Tạo file Excel từ dữ liệu
            using (var package = new ExcelPackage())
            {
                // Sheet 1: Chi tiêu theo CampaignId
                var campaignSheet = package.Workbook.Worksheets.Add("Campaign Expenses");
                campaignSheet.Cells[1, 1].Value = "STT";
                campaignSheet.Cells[1, 2].Value = "Description";
                campaignSheet.Cells[1, 3].Value = "Expense Date";
                campaignSheet.Cells[1, 4].Value = "Amount";
                campaignSheet.Cells[1, 5].Value = "Organiser Id";
                campaignSheet.Cells[1, 6].Value = "Campaign Id";

                int row = 2;
                int stt = 1;
                foreach (var expense in campaignExpenses)
                {
                    campaignSheet.Cells[row, 1].Value = stt;
                    campaignSheet.Cells[row, 2].Value = expense.Description;
                    campaignSheet.Cells[row, 3].Value = expense.ExpenseDate.ToString("yyyy-MM-dd");
                    campaignSheet.Cells[row, 3].Style.Numberformat.Format = "dd-mm-yy";

                    campaignSheet.Cells[row, 4].Value = expense.Amount;
                    campaignSheet.Cells[row, 5].Value = expense.OrganiserId;
                    campaignSheet.Cells[row, 6].Value = expense.CampaignId;

                    row++;
                    stt++;
                }
                campaignSheet.Cells[row, 3].Value = "Total Amount:";
                campaignSheet.Cells[row, 3].Style.Font.Bold = true;
                campaignSheet.Cells[row, 4].Formula = $"SUM(D2:D{row - 1})";
                campaignSheet.Cells[row, 4].Style.Font.Bold = true;

                // Sheet 2: Toàn bộ danh sách chi tiêu theo ngày
                var allExpensesSheet = package.Workbook.Worksheets.Add("All Expenses");
                allExpensesSheet.Cells[1, 1].Value = "STT";
                allExpensesSheet.Cells[1, 2].Value = "Description";
                allExpensesSheet.Cells[1, 3].Value = "Expense Date";
                allExpensesSheet.Cells[1, 4].Value = "Amount";
                allExpensesSheet.Cells[1, 5].Value = "Organiser Id";
                allExpensesSheet.Cells[1, 6].Value = "Campaign Id";

                row = 2;
                stt = 1;
                foreach (var expense in allExpenses)
                {
                    allExpensesSheet.Cells[row, 1].Value = stt;
                    allExpensesSheet.Cells[row, 2].Value = expense.Description;
                    allExpensesSheet.Cells[row, 3].Value = expense.ExpenseDate.ToString("yyyy-MM-dd");
                    allExpensesSheet.Cells[row, 3].Style.Numberformat.Format = "dd-mm-yy";

                    allExpensesSheet.Cells[row, 4].Value = expense.Amount;
                    allExpensesSheet.Cells[row, 5].Value = expense.OrganiserId;
                    allExpensesSheet.Cells[row, 6].Value = expense.CampaignId;

                    row++;
                    stt++;
                }
                allExpensesSheet.Cells[row, 3].Value = "Total Amount:";
                allExpensesSheet.Cells[row, 3].Style.Font.Bold = true;
                allExpensesSheet.Cells[row, 4].Formula = $"SUM(D2:D{row - 1})";
                allExpensesSheet.Cells[row, 4].Style.Font.Bold = true;

                // Lưu file vào MemoryStream và trả về
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                return stream;
            }
        }


        //public MemoryStream GenerateExcelReportExpense(int campaignId, DateTime startDate, DateTime endDate)
        //{
        //    var expense = new List<ExpenseDetail>();

        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();
        //        var query = @"
        //SELECT Id, Description, ExpenseDate, Amount,  OrganiserId, CampaignId
        //FROM Expense
        //WHERE CampaignId = @CampaignId
        //    AND ExpenseDate >= @StartDate 
        //    AND ExpenseDate <= @EndDate";

        //        using (var command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@CampaignId", campaignId);
        //            command.Parameters.AddWithValue("@StartDate", startDate);
        //            command.Parameters.AddWithValue("@EndDate", endDate);

        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    expense.Add(new ExpenseDetail
        //                    {
        //                        Id = reader.GetInt32(0),
        //                        Description = reader.IsDBNull(1) ? null : reader.GetString(1),
        //                        ExpenseDate = reader.GetDateTime(2),
        //                        Amount = reader.IsDBNull(3) ? 0 : Convert.ToDouble(reader.GetDecimal(3)),
        //                        OrganiserId = reader.GetInt32(4),
        //                        CampaignId = reader.GetInt32(5),

        //                    });

        //                }
        //            }
        //        }
        //    }
        //    // Tạo file Excel từ dữ liệu
        //    using (var package = new ExcelPackage())
        //    {
        //        var worksheet = package.Workbook.Worksheets.Add("Expense");
        //        worksheet.Cells[1, 1].Value = "STT";
        //        worksheet.Cells[1, 2].Value = "Description";
        //        worksheet.Cells[1, 3].Value = "ExpenseDate";
        //        worksheet.Cells[1, 4].Value = "Amount";
        //        worksheet.Cells[1, 5].Value = "OrganiserId";
        //        worksheet.Cells[1, 6].Value = "CampaignId";

        //        int row = 2;
        //        int stt = 1;
        //        foreach (var dataex in expense)
        //        {
        //            worksheet.Cells[row, 1].Value = stt;
        //            worksheet.Cells[row, 2].Value = dataex.Description;
        //            worksheet.Cells[row, 3].Value = dataex.ExpenseDate.ToString("yyyy-MM-dd");
        //            worksheet.Cells[row, 3].Style.Numberformat.Format = "dd-mm-yy"; // Định dạng ngày tháng

        //            worksheet.Cells[row, 4].Value = dataex.Amount;
        //            worksheet.Cells[row, 5].Value = dataex.OrganiserId;
        //            worksheet.Cells[row, 6].Value = dataex.CampaignId;

        //            row++;
        //            stt++;
        //        }
        //        var stream = new MemoryStream();
        //        package.SaveAs(stream);
        //        stream.Position = 0;
        //        return stream;
        //    }
        //}
        /*private readonly ICampaignDal _campaignDal;
        private readonly IRateCampaignDal _rateCampaignDal;
        private readonly IImageCampaignDal _imageCampaignDal;
        private readonly IUtilitiesService _utilitiesService;
        private readonly ITransactionDal _transactionDal;

        public CampaignService(ICampaignDal campaignDal, IRateCampaignDal rateCampaignDal, IImageCampaignDal imageCampaignDal, IUtilitiesService utilitiesService, ITransactionDal transactionDal)
        {
            _campaignDal = campaignDal;
            _rateCampaignDal = rateCampaignDal;
            _imageCampaignDal = imageCampaignDal;
            _utilitiesService = utilitiesService;
            _transactionDal = transactionDal;
        }

        public async Task<Campaign?> Get(int campaignId)
        {
            var campaign = await _campaignDal.Get(campaignId);
            if (campaign == null)
            {
                throw new Exception("Cannot find the campaign");
            }
            return campaign;
        }

        public async Task<Campaign> CreateCampaign(CampaignDto campaignDto)
        {
            string coverSrc = "";
            string coverSrcPublicId = "";
            if (campaignDto.CoverImage != null)
            {
                try
                {
                    // Add Campaign cover image to cloudinary
                    var uploadImageResult = await _utilitiesService.CloudinaryUploadPhotoAsync(campaignDto.CoverImage);
                    if (uploadImageResult.Error != null) throw new Exception("Cannot upload cover image on cloudinary");
                    coverSrc = uploadImageResult.SecureUrl.AbsoluteUri;
                    coverSrcPublicId = uploadImageResult.PublicId;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while updating on database" + ex.Message);
                }
            }
            //Create Campaign and add to DTB through CampaignDal.Add
            var campaign = new Campaign()
            {
                Title = campaignDto.Title,
                Target = campaignDto.Target,
                Description = campaignDto.Description,
                Address = campaignDto.Address,
                TargetAmount = campaignDto.TargetAmount,
                Organiser = campaignDto.Organiser,
                CoverSrc = coverSrc,
                CoverSrcPublicId = coverSrcPublicId,
                CreatedDate = DateTime.Now,
                CreatedBy = null,
                Disabled = false,
                UpdatedDate = DateTime.Now,
                UpdatedBy = null,
            };
            await _campaignDal.Add(campaign);
            return campaign;
        }

        public async Task<Campaign> UpdateCampaign(CampaignDto campaignDto)
        {
            var campaign = await _campaignDal.Update(campaignDto);
            if (campaign == null)
            {
                throw new Exception("Cannot find the campaign");
            }
            return campaign;
        }

        public async Task<bool> DeleteCampaign(CampaignDto campaignDto)
        {
            var result = await _transactionDal.CampaignRateImage(campaignDto);
            if (!result)
            {
                throw new Exception("Failed to do delete campaign");
            }
            return result;
        }

        public async Task<bool> ChangeStatusCampaign(int campaignId, int statusId)
        {
            var result = await _campaignDal.ChangeStatus(campaignId, statusId);
            if (!result)
            {
                throw new Exception("Cannot change Campaign's Status");
            }
            return result;
        }

        public async Task<RateCampaign> RateCampaign(RateCampaignDto rateCampaignDto)
        {
            var result = await _rateCampaignDal.CheckExistedRateCampaign(rateCampaignDto);
            if (result)
            {
                throw new Exception("The user has rated this campaign.");
            }
            RateCampaign rateCampaign = new RateCampaign()
            {
                CampaignId = rateCampaignDto.CampaignId,
                Comment = rateCampaignDto.Content,
                Rate = rateCampaignDto.Rate,
                RatedDate = DateTime.Now,
                DonorId = rateCampaignDto.DonorId,
            };
            await _rateCampaignDal.Add(rateCampaign);
            return rateCampaign;
        }
        public async Task<RateCampaign> UpdateRateCampaign(RateCampaignDto rateCampaignDto)
        {
            var rate = await _rateCampaignDal.CheckExistedRateCampaign(rateCampaignDto);
            if (!rate)
            {
                throw new Exception("The user has not rated this campaign.");
            }
            var result = await _rateCampaignDal.Update(rateCampaignDto);
            if (result == null) throw new Exception("Failed to update rate campaign.");
            return result;
        }

        public async Task<List<ImageCampaign>> AddListImageCampaign(List<ImageCampaignDto> listImageCampaignDto)
        {
            List<ImageCampaign> imageCampaigns = new List<ImageCampaign>();
            if (listImageCampaignDto == null || listImageCampaignDto.Any()) throw new Exception("No images provided");
            foreach (var imageDto in listImageCampaignDto)
            {
                try
                {
                    var uploadImageResult = await _utilitiesService.CloudinaryUploadPhotoAsync(imageDto.ImageCampaign);
                    if (uploadImageResult.Error != null)
                        throw new Exception("Failed to upload image to Cloudinary");

                    // Create a new CampaignImage object and add it to the list
                    var imageCampaign = new ImageCampaign
                    {
                        CampaignId = imageDto.CampaignId,
                        ImageSrc = uploadImageResult.SecureUrl.AbsoluteUri,
                        ImageSrcPublicId = uploadImageResult.PublicId,
                        StatusCampaignId = imageDto.StatusCampaignId
                    };
                    imageCampaigns.Add(imageCampaign);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error uploading image: {ex.Message}");
                }
            }
            return imageCampaigns;
        }
        public async Task<bool> RemoveListImageCampaign(List<ImageCampaignDto> listImageCampaignDto)
        {
            var result = await _imageCampaignDal.RemoveListImages(listImageCampaignDto);
            if(result == false)
            {
                throw new Exception("Can not find the Image");
            }
            return result;
        }*/
    }
}