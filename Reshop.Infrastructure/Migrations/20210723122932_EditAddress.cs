using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class EditAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProducts_ShopperProducts_ShopperProductId",
                table: "FavoriteProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Shoppers_ShopperId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopperProductDiscounts_ShopperProducts_ShopperProductId",
                table: "ShopperProductDiscounts");

            migrationBuilder.DropIndex(
                name: "IX_ShopperProductDiscounts_ShopperProductId",
                table: "ShopperProductDiscounts");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ShopperId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteProducts_ShopperProductId",
                table: "FavoriteProducts");

            migrationBuilder.DropColumn(
                name: "ShopperProductId",
                table: "ShopperProductDiscounts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ShopperId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ShopperProductId",
                table: "FavoriteProducts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShopperProductId",
                table: "ShopperProductDiscounts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ShopperId",
                table: "OrderDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShopperProductId",
                table: "FavoriteProducts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductDiscounts_ShopperProductId",
                table: "ShopperProductDiscounts",
                column: "ShopperProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ShopperId",
                table: "OrderDetails",
                column: "ShopperId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_ShopperProductId",
                table: "FavoriteProducts",
                column: "ShopperProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProducts_ShopperProducts_ShopperProductId",
                table: "FavoriteProducts",
                column: "ShopperProductId",
                principalTable: "ShopperProducts",
                principalColumn: "ShopperProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Shoppers_ShopperId",
                table: "OrderDetails",
                column: "ShopperId",
                principalTable: "Shoppers",
                principalColumn: "ShopperId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopperProductDiscounts_ShopperProducts_ShopperProductId",
                table: "ShopperProductDiscounts",
                column: "ShopperProductId",
                principalTable: "ShopperProducts",
                principalColumn: "ShopperProductId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
