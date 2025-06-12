using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DonationAppDemo.Migrations
{
    public partial class _11_AddRecipientIdAndReceivedAttributeToCampaignEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Received",
                table: "Campaign",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecipientId",
                table: "Campaign",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_RecipientId",
                table: "Campaign",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaign_Recipient_RecipientId",
                table: "Campaign",
                column: "RecipientId",
                principalTable: "Recipient",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaign_Recipient_RecipientId",
                table: "Campaign");

            migrationBuilder.DropIndex(
                name: "IX_Campaign_RecipientId",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "Received",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "Campaign");
        }
    }
}
