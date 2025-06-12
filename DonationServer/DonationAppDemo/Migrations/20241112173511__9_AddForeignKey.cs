using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DonationAppDemo.Migrations
{
    public partial class _9_AddForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Post",
                newName: "AdminId");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Organiser",
                type: "varchar(13)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(13)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Donor",
                type: "varchar(13)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(13)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Admin",
                type: "varchar(13)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(13)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transference_AdminId",
                table: "Transference",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Transference_CampaignId",
                table: "Transference",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_RateCampaign_DonorId",
                table: "RateCampaign",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_AdminId",
                table: "Post",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Organiser_AccountId",
                table: "Organiser",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagePost_PostId",
                table: "ImagePost",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageCommentPost_CommentPostId",
                table: "ImageCommentPost",
                column: "CommentPostId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageCampaign_CampaignId",
                table: "ImageCampaign",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageCampaign_StatusCampaignId",
                table: "ImageCampaign",
                column: "StatusCampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_CampaignId",
                table: "Expense",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_OrganiserId",
                table: "Expense",
                column: "OrganiserId");

            migrationBuilder.CreateIndex(
                name: "IX_Donor_AccountId",
                table: "Donor",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Donation_CampaignId",
                table: "Donation",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Donation_DonorId",
                table: "Donation",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_Donation_PaymentMethodId",
                table: "Donation",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentPost_PostId",
                table: "CommentPost",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignParticipant_DonorId",
                table: "CampaignParticipant",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_OrganiserId",
                table: "Campaign",
                column: "OrganiserId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_StatusCampaignId",
                table: "Campaign",
                column: "StatusCampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Admin_AccountId",
                table: "Admin",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_Account_AccountId",
                table: "Admin",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "PhoneNum");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaign_Organiser_OrganiserId",
                table: "Campaign",
                column: "OrganiserId",
                principalTable: "Organiser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaign_StatusCampaign_StatusCampaignId",
                table: "Campaign",
                column: "StatusCampaignId",
                principalTable: "StatusCampaign",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignParticipant_Campaign_CampaignId",
                table: "CampaignParticipant",
                column: "CampaignId",
                principalTable: "Campaign",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignParticipant_Donor_DonorId",
                table: "CampaignParticipant",
                column: "DonorId",
                principalTable: "Donor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentPost_Post_PostId",
                table: "CommentPost",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Donation_Campaign_CampaignId",
                table: "Donation",
                column: "CampaignId",
                principalTable: "Campaign",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Donation_Donor_DonorId",
                table: "Donation",
                column: "DonorId",
                principalTable: "Donor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Donation_PaymentMethod_PaymentMethodId",
                table: "Donation",
                column: "PaymentMethodId",
                principalTable: "PaymentMethod",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Donor_Account_AccountId",
                table: "Donor",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "PhoneNum");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Campaign_CampaignId",
                table: "Expense",
                column: "CampaignId",
                principalTable: "Campaign",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Organiser_OrganiserId",
                table: "Expense",
                column: "OrganiserId",
                principalTable: "Organiser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageCampaign_Campaign_CampaignId",
                table: "ImageCampaign",
                column: "CampaignId",
                principalTable: "Campaign",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageCampaign_StatusCampaign_StatusCampaignId",
                table: "ImageCampaign",
                column: "StatusCampaignId",
                principalTable: "StatusCampaign",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageCommentPost_CommentPost_CommentPostId",
                table: "ImageCommentPost",
                column: "CommentPostId",
                principalTable: "CommentPost",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImagePost_Post_PostId",
                table: "ImagePost",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Organiser_Account_AccountId",
                table: "Organiser",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "PhoneNum");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Admin_AdminId",
                table: "Post",
                column: "AdminId",
                principalTable: "Admin",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RateCampaign_Campaign_CampaignId",
                table: "RateCampaign",
                column: "CampaignId",
                principalTable: "Campaign",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RateCampaign_Donor_DonorId",
                table: "RateCampaign",
                column: "DonorId",
                principalTable: "Donor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transference_Admin_AdminId",
                table: "Transference",
                column: "AdminId",
                principalTable: "Admin",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transference_Campaign_CampaignId",
                table: "Transference",
                column: "CampaignId",
                principalTable: "Campaign",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_Account_AccountId",
                table: "Admin");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaign_Organiser_OrganiserId",
                table: "Campaign");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaign_StatusCampaign_StatusCampaignId",
                table: "Campaign");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignParticipant_Campaign_CampaignId",
                table: "CampaignParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignParticipant_Donor_DonorId",
                table: "CampaignParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentPost_Post_PostId",
                table: "CommentPost");

            migrationBuilder.DropForeignKey(
                name: "FK_Donation_Campaign_CampaignId",
                table: "Donation");

            migrationBuilder.DropForeignKey(
                name: "FK_Donation_Donor_DonorId",
                table: "Donation");

            migrationBuilder.DropForeignKey(
                name: "FK_Donation_PaymentMethod_PaymentMethodId",
                table: "Donation");

            migrationBuilder.DropForeignKey(
                name: "FK_Donor_Account_AccountId",
                table: "Donor");

            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Campaign_CampaignId",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Organiser_OrganiserId",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageCampaign_Campaign_CampaignId",
                table: "ImageCampaign");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageCampaign_StatusCampaign_StatusCampaignId",
                table: "ImageCampaign");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageCommentPost_CommentPost_CommentPostId",
                table: "ImageCommentPost");

            migrationBuilder.DropForeignKey(
                name: "FK_ImagePost_Post_PostId",
                table: "ImagePost");

            migrationBuilder.DropForeignKey(
                name: "FK_Organiser_Account_AccountId",
                table: "Organiser");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Admin_AdminId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_RateCampaign_Campaign_CampaignId",
                table: "RateCampaign");

            migrationBuilder.DropForeignKey(
                name: "FK_RateCampaign_Donor_DonorId",
                table: "RateCampaign");

            migrationBuilder.DropForeignKey(
                name: "FK_Transference_Admin_AdminId",
                table: "Transference");

            migrationBuilder.DropForeignKey(
                name: "FK_Transference_Campaign_CampaignId",
                table: "Transference");

            migrationBuilder.DropIndex(
                name: "IX_Transference_AdminId",
                table: "Transference");

            migrationBuilder.DropIndex(
                name: "IX_Transference_CampaignId",
                table: "Transference");

            migrationBuilder.DropIndex(
                name: "IX_RateCampaign_DonorId",
                table: "RateCampaign");

            migrationBuilder.DropIndex(
                name: "IX_Post_AdminId",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Organiser_AccountId",
                table: "Organiser");

            migrationBuilder.DropIndex(
                name: "IX_ImagePost_PostId",
                table: "ImagePost");

            migrationBuilder.DropIndex(
                name: "IX_ImageCommentPost_CommentPostId",
                table: "ImageCommentPost");

            migrationBuilder.DropIndex(
                name: "IX_ImageCampaign_CampaignId",
                table: "ImageCampaign");

            migrationBuilder.DropIndex(
                name: "IX_ImageCampaign_StatusCampaignId",
                table: "ImageCampaign");

            migrationBuilder.DropIndex(
                name: "IX_Expense_CampaignId",
                table: "Expense");

            migrationBuilder.DropIndex(
                name: "IX_Expense_OrganiserId",
                table: "Expense");

            migrationBuilder.DropIndex(
                name: "IX_Donor_AccountId",
                table: "Donor");

            migrationBuilder.DropIndex(
                name: "IX_Donation_CampaignId",
                table: "Donation");

            migrationBuilder.DropIndex(
                name: "IX_Donation_DonorId",
                table: "Donation");

            migrationBuilder.DropIndex(
                name: "IX_Donation_PaymentMethodId",
                table: "Donation");

            migrationBuilder.DropIndex(
                name: "IX_CommentPost_PostId",
                table: "CommentPost");

            migrationBuilder.DropIndex(
                name: "IX_CampaignParticipant_DonorId",
                table: "CampaignParticipant");

            migrationBuilder.DropIndex(
                name: "IX_Campaign_OrganiserId",
                table: "Campaign");

            migrationBuilder.DropIndex(
                name: "IX_Campaign_StatusCampaignId",
                table: "Campaign");

            migrationBuilder.DropIndex(
                name: "IX_Admin_AccountId",
                table: "Admin");

            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "Post",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "UserRole",
                table: "Post",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Organiser",
                type: "varchar(13)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(13)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Donor",
                type: "varchar(13)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(13)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Admin",
                type: "varchar(13)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(13)",
                oldNullable: true);
        }
    }
}
