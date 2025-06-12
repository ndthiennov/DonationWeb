using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DonationAppDemo.Migrations
{
    public partial class _12_AddRatedByRecipientInCampaignEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RatedByRecipient",
                table: "Campaign",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RatedContentByRecipient",
                table: "Campaign",
                type: "nvarchar(250)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatedByRecipient",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "RatedContentByRecipient",
                table: "Campaign");
        }
    }
}
