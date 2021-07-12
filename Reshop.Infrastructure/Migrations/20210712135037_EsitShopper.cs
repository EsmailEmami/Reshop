using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class EsitShopper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EditShopperProducts_Users_ShopperUserId",
                table: "EditShopperProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Users_ShopperUserId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopperProducts_Shoppers_ShopperId",
                table: "ShopperProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopperProducts_Users_ShopperUserId",
                table: "ShopperProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopperProducts",
                table: "ShopperProducts");

            migrationBuilder.DropIndex(
                name: "IX_ShopperProducts_ShopperId",
                table: "ShopperProducts");

            migrationBuilder.DropIndex(
                name: "IX_EditShopperProducts_ShopperUserId",
                table: "EditShopperProducts");

            migrationBuilder.DropColumn(
                name: "ShopperUserId",
                table: "ShopperProducts");

            migrationBuilder.DropColumn(
                name: "ShopperUserId",
                table: "FavoriteProducts");

            migrationBuilder.DropColumn(
                name: "ShopperUserId",
                table: "EditShopperProducts");

            migrationBuilder.RenameColumn(
                name: "ShopperUserId",
                table: "OrderDetails",
                newName: "ShopperId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ShopperUserId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ShopperId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Shoppers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ShopperId",
                table: "ShopperProducts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShopperId",
                table: "FavoriteProducts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopperProducts",
                table: "ShopperProducts",
                columns: new[] { "ShopperId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_Shoppers_UserId",
                table: "Shoppers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_ShopperId",
                table: "FavoriteProducts",
                column: "ShopperId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProducts_Shoppers_ShopperId",
                table: "FavoriteProducts",
                column: "ShopperId",
                principalTable: "Shoppers",
                principalColumn: "ShopperId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Shoppers_ShopperId",
                table: "OrderDetails",
                column: "ShopperId",
                principalTable: "Shoppers",
                principalColumn: "ShopperId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopperProducts_Shoppers_ShopperId",
                table: "ShopperProducts",
                column: "ShopperId",
                principalTable: "Shoppers",
                principalColumn: "ShopperId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shoppers_Users_UserId",
                table: "Shoppers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProducts_Shoppers_ShopperId",
                table: "FavoriteProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Shoppers_ShopperId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopperProducts_Shoppers_ShopperId",
                table: "ShopperProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shoppers_Users_UserId",
                table: "Shoppers");

            migrationBuilder.DropIndex(
                name: "IX_Shoppers_UserId",
                table: "Shoppers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopperProducts",
                table: "ShopperProducts");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteProducts_ShopperId",
                table: "FavoriteProducts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Shoppers");

            migrationBuilder.DropColumn(
                name: "ShopperId",
                table: "FavoriteProducts");

            migrationBuilder.RenameColumn(
                name: "ShopperId",
                table: "OrderDetails",
                newName: "ShopperUserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ShopperId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ShopperUserId");

            migrationBuilder.AlterColumn<string>(
                name: "ShopperId",
                table: "ShopperProducts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ShopperUserId",
                table: "ShopperProducts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShopperUserId",
                table: "FavoriteProducts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShopperUserId",
                table: "EditShopperProducts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopperProducts",
                table: "ShopperProducts",
                columns: new[] { "ShopperUserId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProducts_ShopperId",
                table: "ShopperProducts",
                column: "ShopperId");

            migrationBuilder.CreateIndex(
                name: "IX_EditShopperProducts_ShopperUserId",
                table: "EditShopperProducts",
                column: "ShopperUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EditShopperProducts_Users_ShopperUserId",
                table: "EditShopperProducts",
                column: "ShopperUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Users_ShopperUserId",
                table: "OrderDetails",
                column: "ShopperUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopperProducts_Shoppers_ShopperId",
                table: "ShopperProducts",
                column: "ShopperId",
                principalTable: "Shoppers",
                principalColumn: "ShopperId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopperProducts_Users_ShopperUserId",
                table: "ShopperProducts",
                column: "ShopperUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
