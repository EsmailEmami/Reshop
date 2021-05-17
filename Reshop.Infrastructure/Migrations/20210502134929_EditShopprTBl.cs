using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class EditShopprTBl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailActive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FatherFullName",
                table: "Shoppers");

            migrationBuilder.RenameColumn(
                name: "ShopperWithNationalCardImageName",
                table: "Shoppers",
                newName: "BusinessLicenseImageName");

            migrationBuilder.AddColumn<string>(
                name: "LandlinePhoneNumber",
                table: "Shoppers",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StoreName",
                table: "Shoppers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PayDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Plaque",
                table: "Addresses",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "StoreTitles",
                columns: table => new
                {
                    StoreTitleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreTitleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTitles", x => x.StoreTitleId);
                });

            migrationBuilder.CreateTable(
                name: "ShopperTitles",
                columns: table => new
                {
                    ShopperId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StoreTitleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopperTitles", x => new { x.ShopperId, x.StoreTitleId });
                    table.ForeignKey(
                        name: "FK_ShopperTitles_Shoppers_ShopperId",
                        column: x => x.ShopperId,
                        principalTable: "Shoppers",
                        principalColumn: "ShopperId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopperTitles_StoreTitles_StoreTitleId",
                        column: x => x.StoreTitleId,
                        principalTable: "StoreTitles",
                        principalColumn: "StoreTitleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopperTitles_StoreTitleId",
                table: "ShopperTitles",
                column: "StoreTitleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopperTitles");

            migrationBuilder.DropTable(
                name: "StoreTitles");

            migrationBuilder.DropColumn(
                name: "LandlinePhoneNumber",
                table: "Shoppers");

            migrationBuilder.DropColumn(
                name: "StoreName",
                table: "Shoppers");

            migrationBuilder.DropColumn(
                name: "PayDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Plaque",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "BusinessLicenseImageName",
                table: "Shoppers",
                newName: "ShopperWithNationalCardImageName");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FatherFullName",
                table: "Shoppers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
