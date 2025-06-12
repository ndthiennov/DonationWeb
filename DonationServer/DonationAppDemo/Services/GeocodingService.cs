using DonationAppDemo.DTOs;
using DonationAppDemo.Models;
using DonationAppDemo.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Twilio.TwiML.Voice;
using Task = System.Threading.Tasks.Task;

namespace DonationAppDemo.Services
{
    public class GeocodingService: IGeocodingService
    {
        private readonly string _connectionString;

        public GeocodingService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DonationDbConnection");
        }
        // Hàm xử lý lấy tọa độ và chèn vào Locations
        public async Task<int> ProcessGeocodingAsync(DateTime startDate, DateTime endDate)
        {
            int processedCount = 0;
            var addressesToGeocode = await GetAddressesNotGeocodedAsync(startDate, endDate);

            foreach (var location in addressesToGeocode)
            {
                try
                {
                    Console.WriteLine($"Processing address: {location.Add_Campaign}");

                    var coordinates = await GetCoordinatesFromAddress(location.Add_Campaign);
                    if (coordinates != null)
                    {
                        Console.WriteLine($"Coordinates for {location.Add_Campaign}: Lat={coordinates.Value.Latitude}, Lng={coordinates.Value.Longitude}");
                        await InsertLocationAsync(location, coordinates.Value.Latitude, coordinates.Value.Longitude);
                        processedCount++;
                    }
                    else
                    {
                        Console.WriteLine($"No coordinates found for address: {location.Add_Campaign}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing address {location.Add_Campaign}: {ex.Message}");
                }
            }
            return processedCount;
        }
        // Hàm lấy các địa chỉ chưa được xử lý
        private async Task<List<Locations>> GetAddressesNotGeocodedAsync(DateTime startDate, DateTime endDate)
        {
            var results = new List<Locations>();
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = @"
            SELECT 
                c.Id AS CampaignId,
                c.Address AS CampaignAddress,
                c.CreatedDate,
                l.Id AS LocationId
            FROM 
                Campaign c
            LEFT JOIN 
                Locations l 
            ON 
                c.Id = l.Id_Campaign -- Đảm bảo mối quan hệ khóa ngoại
            WHERE 
                c.CreatedDate BETWEEN @startDate AND @endDate";

            await using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                if (reader["LocationId"] == DBNull.Value) // Nếu LocationId NULL, địa chỉ chưa được xử lý
                {
                    results.Add(new Locations
                    {
                        Id_Campaign = reader.GetInt32(0), // CampaignId
                        Add_Campaign = reader.GetString(1), // CampaignAddress
                        Created_date = reader.GetDateTime(2) // CreatedDate
                    });
                }
            }
            return results;
        }
        // Hàm lấy tọa độ từ API Nominatim
        private async Task<(float Latitude, float Longitude)?> GetCoordinatesFromAddress(string address)
        {
            using var client = new HttpClient();

            string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json";

            client.DefaultRequestHeaders.Add("User-Agent", "DonationAppDemo/1.0 (contact@example.com)");

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var json = JArray.Parse(responseContent);

                    if (json.Count > 0)
                    {
                        var location = json[0];
                        float latitude = float.Parse(location["lat"]?.ToString() ?? "0");
                        float longitude = float.Parse(location["lon"]?.ToString() ?? "0");
                        return (latitude, longitude);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling Nominatim API for address '{address}': {ex.Message}");
            }

            return null;
        }

        // Hàm chèn dữ liệu vào bảng Locations
        private async Task InsertLocationAsync(Locations location, float latitude, float longitude)
        {
            string insertQuery = @"
        INSERT INTO Locations (Add_Campaign, Latitude, Longitude, Created_date, Id_Campaign)
        VALUES (@AddCampaign, @Latitude, @Longitude, @CreatedDate, @IdCampaign)";

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            try
            {
                await using var cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@AddCampaign", location.Add_Campaign);
                cmd.Parameters.AddWithValue("@Latitude", latitude);
                cmd.Parameters.AddWithValue("@Longitude", longitude);
                cmd.Parameters.AddWithValue("@CreatedDate", location.Created_date);
                cmd.Parameters.AddWithValue("@IdCampaign", location.Id_Campaign);
                await cmd.ExecuteNonQueryAsync();
                Console.WriteLine($"Successfully inserted location: {location.Add_Campaign}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting location: {ex.Message}");
                throw; // Hoặc ghi lại để xử lý sau
            }
        }
        
       //Lọc Locations theo khoảng thời gian
        public async Task<List<Locations>> GetLocationsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            await ProcessGeocodingAsync(startDate,endDate);
            var results = new List<Locations>();

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            // Cập nhật câu lệnh SQL để lọc theo ngày
            string query = @"
                SELECT Id, Add_Campaign, Latitude, Longitude, Created_date
                FROM Locations
                WHERE Created_date >= @StartDate AND Created_date <= @EndDate";

            await using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new Locations
                {
                    Id = reader.GetInt32(0),
                    Add_Campaign = reader.GetString(1),
                    Latitude = reader.GetDouble(2),
                    Longitude = reader.GetDouble(3),
                    Created_date = reader.GetDateTime(4)
                });
            }
            return results;
        }        
    }
}

