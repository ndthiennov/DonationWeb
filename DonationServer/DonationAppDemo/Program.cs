using DonationAppDemo.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using DonationAppDemo.Services;
using DonationAppDemo.DAL.Interfaces;
using DonationAppDemo.Services.Interfaces;
using DonationAppDemo.HubConfig;
using System.ComponentModel;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database connection
builder.Services.AddDbContext<DonationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DonationDbConnection")));

// Jwt
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(option =>
    {
        option.SaveToken = true;
        option.RequireHttpsMetadata = false;
        option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });

builder.Services.AddControllers(options =>
{
    options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
    options.OutputFormatters.Add(new SystemTextJsonOutputFormatter(new JsonSerializerOptions(JsonSerializerDefaults.Web)
    {
        ReferenceHandler = ReferenceHandler.Preserve,
    }));
});

// Interface
/*Dal*/
builder.Services.AddTransient<IAccountDal, AccountDal>();
builder.Services.AddTransient<IAdminDal, AdminDal>();
builder.Services.AddTransient<IOrganiserDal, OrganiserDal>();
builder.Services.AddTransient<IDonorDal, DonorDal>();
builder.Services.AddTransient<IUserTokenDal, UserTokenDal>();
builder.Services.AddTransient<ICampaignDal, CampaignDal>();
builder.Services.AddTransient<ICampaignParticipantDal, CampaignParticipantDal>();
builder.Services.AddTransient<IImageCampaignDal, ImageCampaignDal>();
builder.Services.AddTransient<IRateCampaignDal, RateCampaignDal>();
builder.Services.AddTransient<ITransactionDal, TransactionDal>();
builder.Services.AddTransient<IDonationDal, DonationDal>();
builder.Services.AddTransient<ICampaignStatisticsDal, CampaignStatisticsDal>();
builder.Services.AddTransient<INotificationDal, NotificationDal>();
builder.Services.AddTransient<IRecipientDal, RecipientDal>();
builder.Services.AddTransient<IPostDal, PostDal>();
builder.Services.AddTransient<ICommentDal, CommentDal>();
builder.Services.AddTransient<IImagePostDal, ImagePostDal>();
builder.Services.AddTransient<IExpenseDal, ExpenseDal>();

/*Service*/
builder.Services.AddTransient<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<IDonorService, DonorService>();
builder.Services.AddTransient<IOrganiserService, OrganiserService>();
builder.Services.AddTransient<IUserTokenService, UserTokenService>();
builder.Services.AddTransient<IUtilitiesService, UtilitiesService>();
builder.Services.AddTransient<ICampaignService, CampaignService>();
builder.Services.AddTransient<ICampaignStatisticsService, CampaignStatisticsService>();
builder.Services.AddTransient<ICampaignParticipantService, CampaignParticipantService>();
builder.Services.AddTransient<IImageCampaignService, ImageCampaignService>();
builder.Services.AddTransient<IDonationService, DonationService>();
builder.Services.AddTransient<IDonationHubService, DonationHubService>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddTransient<IRecipientService, RecipientService>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IGeocodingService, GeocodingService>();
builder.Services.AddScoped<IRateCampaignService, RateCampaignService>();

//builder.Services.AddHostedService<GeocodingBackgroundService>();
// HttpContext
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//ExcellPackage
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Cors
builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// SignalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<DonationHub>("/donationhub");

app.Run();
