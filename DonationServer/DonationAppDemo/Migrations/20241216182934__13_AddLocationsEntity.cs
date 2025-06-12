using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DonationAppDemo.Migrations
{
    public partial class _13_AddLocationsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Campaign_CampaignId",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Organiser_OrganiserId",
                table: "Expense");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostDate",
                table: "Post",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "ImagePost",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "OrganiserId",
                table: "Expense",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CampaignId",
                table: "Expense",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Add_Campaign = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Created_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Id_Campaign = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Campaign_CampaignId",
                table: "Expense",
                column: "CampaignId",
                principalTable: "Campaign",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Organiser_OrganiserId",
                table: "Expense",
                column: "OrganiserId",
                principalTable: "Organiser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Campaign_CampaignId",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Organiser_OrganiserId",
                table: "Expense");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "ImagePost");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostDate",
                table: "Post",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "OrganiserId",
                table: "Expense",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CampaignId",
                table: "Expense",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
        }
    }
}
