using CloudinaryDotNet;
using DonationAppDemo.Helper;
using DonationAppDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.SqlTypes;

namespace DonationAppDemo.DAL
{
    public class DonationDbContext : DbContext
    {
        public DonationDbContext(DbContextOptions<DonationDbContext> options) : base(options) { }
        public virtual DbSet<DonationAppDemo.Models.Account> Account { get; set; } = null!;
        public virtual DbSet<UserToken> UserToken { get; set; } = null!;
        public virtual DbSet<Admin> Admin { get; set; } = null!;
        public virtual DbSet<Campaign> Campaign { get; set; } = null!;
        public virtual DbSet<CampaignParticipant> CampaignParticipant { get; set; } = null!;
        public virtual DbSet<CommentPost> CommentPost { get; set; } = null!;
        public virtual DbSet<Donation> Donation { get; set; } = null!;
        public virtual DbSet<Transference> Transference { get; set; } = null!;
        public virtual DbSet<CampaignStatistics> CampaignStatistics { get; set; } = null!;
        public virtual DbSet<PaymentMethod> PaymentMethod { get; set; } = null!;
        public virtual DbSet<Donor> Donor { get; set; } = null!;
        public virtual DbSet<ImageCampaign> ImageCampaign { get; set; } = null!;
        public virtual DbSet<ImageCommentPost> ImageCommentPost { get; set; } = null!;
        public virtual DbSet<ImagePost> ImagePost { get; set; } = null!;
        public virtual DbSet<Notification> Notification { get; set; } = null!;
        public virtual DbSet<Organiser> Organiser { get; set; } = null!;
        public virtual DbSet<Expense> Expense { get; set; } = null!;
        public virtual DbSet<Post> Post { get; set; } = null!;
        public virtual DbSet<RateCampaign> RateCampaign { get; set; } = null!;
        public virtual DbSet<StatusCampaign> StatusCampaign { get; set; } = null!;
        public virtual DbSet<Recipient> Recipient { get; set; } = null!;
        public virtual DbSet<Locations> Locations { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add multiple primary keys
            modelBuilder.Entity<RateCampaign>().HasKey(x => new
            {
                x.CampaignId,
                x.DonorId
            });

            modelBuilder.Entity<CampaignParticipant>().HasKey(x => new
            {
                x.CampaignId,
                x.DonorId
            });

            // Add index
            modelBuilder.Entity<Admin>()
                .HasIndex(u => u.Name)
                .HasDatabaseName("IX_Admin_Name");

            modelBuilder.Entity<Organiser>()
                .HasIndex(u => u.Name)
                .HasDatabaseName("IX_Organiser_Name");

            modelBuilder.Entity<Donor>()
                .HasIndex(u => u.Name)
                .HasDatabaseName("IX_Donor_Name");

            // Seed data
        }
    }
}
