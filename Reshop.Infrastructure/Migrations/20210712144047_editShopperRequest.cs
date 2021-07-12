using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class editShopperRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EditShopperProducts");

            migrationBuilder.CreateTable(
                name: "ShopperProductRequests",
                columns: table => new
                {
                    ShopperProductRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ShopperId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestType = table.Column<bool>(type: "bit", nullable: false),
                    Warranty = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "Money", nullable: false),
                    QuantityInStock = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopperProductRequests", x => x.ShopperProductRequestId);
                    table.ForeignKey(
                        name: "FK_ShopperProductRequests_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopperProductRequests_Shoppers_ShopperId",
                        column: x => x.ShopperId,
                        principalTable: "Shoppers",
                        principalColumn: "ShopperId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopperProductRequests_Users_RequestUserId",
                        column: x => x.RequestUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductRequests_ProductId",
                table: "ShopperProductRequests",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductRequests_RequestUserId",
                table: "ShopperProductRequests",
                column: "RequestUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductRequests_ShopperId",
                table: "ShopperProductRequests",
                column: "ShopperId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopperProductRequests");

            migrationBuilder.CreateTable(
                name: "EditShopperProducts",
                columns: table => new
                {
                    EditShopperProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "Money", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    QuantityInStock = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShopperId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Warranty = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_EditShopperProducts_ProductId",
                table: "EditShopperProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_EditShopperProducts_ShopperId",
                table: "EditShopperProducts",
                column: "ShopperId");
        }
    }
}
