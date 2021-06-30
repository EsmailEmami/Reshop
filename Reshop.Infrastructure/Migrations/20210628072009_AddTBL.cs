using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class AddTBL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllSalesCount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AllViewsCount",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "EditShopperProducts",
                columns: table => new
                {
                    EditShopperProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ShopperUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Warranty = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "Money", nullable: false),
                    QuantityInStock = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShopperId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditShopperProducts", x => x.EditShopperProductId);
                    table.ForeignKey(
                        name: "FK_EditShopperProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EditShopperProducts_Shoppers_ShopperId",
                        column: x => x.ShopperId,
                        principalTable: "Shoppers",
                        principalColumn: "ShopperId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EditShopperProducts_Users_ShopperUserId",
                        column: x => x.ShopperUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EditShopperProducts_ProductId",
                table: "EditShopperProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_EditShopperProducts_ShopperId",
                table: "EditShopperProducts",
                column: "ShopperId");

            migrationBuilder.CreateIndex(
                name: "IX_EditShopperProducts_ShopperUserId",
                table: "EditShopperProducts",
                column: "ShopperUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EditShopperProducts");

            migrationBuilder.AddColumn<int>(
                name: "AllSalesCount",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AllViewsCount",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
