using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class editFull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopperProductRequests_Colors_ColorId",
                table: "ShopperProductRequests");

            migrationBuilder.DropIndex(
                name: "IX_ShopperProductRequests_ColorId",
                table: "ShopperProductRequests");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "ShopperProductRequests");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ShopperProductRequests");

            migrationBuilder.DropColumn(
                name: "QuantityInStock",
                table: "ShopperProductRequests");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "ShopperProductRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShopperProductId",
                table: "ShopperProductRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShopperProductColorRequests",
                columns: table => new
                {
                    ShopperProductColorRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopperProductColorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestType = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "Money", nullable: false),
                    QuantityInStock = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopperProductColorRequests", x => x.ShopperProductColorRequestId);
                    table.ForeignKey(
                        name: "FK_ShopperProductColorRequests_ShopperProductColors_ShopperProductColorId",
                        column: x => x.ShopperProductColorId,
                        principalTable: "ShopperProductColors",
                        principalColumn: "ShopperProductColorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopperProductColorRequests_Users_RequestUserId",
                        column: x => x.RequestUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductRequests_ShopperProductId",
                table: "ShopperProductRequests",
                column: "ShopperProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductColorRequests_RequestUserId",
                table: "ShopperProductColorRequests",
                column: "RequestUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductColorRequests_ShopperProductColorId",
                table: "ShopperProductColorRequests",
                column: "ShopperProductColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopperProductRequests_ShopperProducts_ShopperProductId",
                table: "ShopperProductRequests",
                column: "ShopperProductId",
                principalTable: "ShopperProducts",
                principalColumn: "ShopperProductId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopperProductRequests_ShopperProducts_ShopperProductId",
                table: "ShopperProductRequests");

            migrationBuilder.DropTable(
                name: "ShopperProductColorRequests");

            migrationBuilder.DropIndex(
                name: "IX_ShopperProductRequests_ShopperProductId",
                table: "ShopperProductRequests");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "ShopperProductRequests");

            migrationBuilder.DropColumn(
                name: "ShopperProductId",
                table: "ShopperProductRequests");

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "ShopperProductRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ShopperProductRequests",
                type: "Money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "QuantityInStock",
                table: "ShopperProductRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductRequests_ColorId",
                table: "ShopperProductRequests",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopperProductRequests_Colors_ColorId",
                table: "ShopperProductRequests",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "ColorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
