using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class editShopperTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentAnswers");

            migrationBuilder.DropColumn(
                name: "DiscountPercent",
                table: "ShopperProducts");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "ShopperProducts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ShopperProducts");

            migrationBuilder.DropColumn(
                name: "QuantityInStock",
                table: "ShopperProducts");

            migrationBuilder.DropColumn(
                name: "SaleCount",
                table: "ShopperProducts");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "ShopperProducts");

            migrationBuilder.RenameColumn(
                name: "IsInDiscount",
                table: "ShopperProducts",
                newName: "IsActive");

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "ShopperProductRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ShopperProductColorId",
                table: "ShopperProductDiscounts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShopperProductColorId",
                table: "OrderDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShopperProductColorId",
                table: "FavoriteProducts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    ColorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColorName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.ColorId);
                });

            migrationBuilder.CreateTable(
                name: "ShopperProductColors",
                columns: table => new
                {
                    ShopperProductColorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopperProductId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ColorId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "Money", nullable: false),
                    QuantityInStock = table.Column<int>(type: "int", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    SaleCount = table.Column<int>(type: "int", nullable: false),
                    IsFinally = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopperProductColors", x => x.ShopperProductColorId);
                    table.ForeignKey(
                        name: "FK_ShopperProductColors_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "ColorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopperProductColors_ShopperProducts_ShopperProductId",
                        column: x => x.ShopperProductId,
                        principalTable: "ShopperProducts",
                        principalColumn: "ShopperProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductRequests_ColorId",
                table: "ShopperProductRequests",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductDiscounts_ShopperProductColorId",
                table: "ShopperProductDiscounts",
                column: "ShopperProductColorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ShopperProductColorId",
                table: "OrderDetails",
                column: "ShopperProductColorId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_ShopperProductColorId",
                table: "FavoriteProducts",
                column: "ShopperProductColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductColors_ColorId",
                table: "ShopperProductColors",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductColors_ShopperProductId",
                table: "ShopperProductColors",
                column: "ShopperProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProducts_ShopperProductColors_ShopperProductColorId",
                table: "FavoriteProducts",
                column: "ShopperProductColorId",
                principalTable: "ShopperProductColors",
                principalColumn: "ShopperProductColorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ShopperProductColors_ShopperProductColorId",
                table: "OrderDetails",
                column: "ShopperProductColorId",
                principalTable: "ShopperProductColors",
                principalColumn: "ShopperProductColorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopperProductDiscounts_ShopperProductColors_ShopperProductColorId",
                table: "ShopperProductDiscounts",
                column: "ShopperProductColorId",
                principalTable: "ShopperProductColors",
                principalColumn: "ShopperProductColorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopperProductRequests_Colors_ColorId",
                table: "ShopperProductRequests",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "ColorId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProducts_ShopperProductColors_ShopperProductColorId",
                table: "FavoriteProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ShopperProductColors_ShopperProductColorId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopperProductDiscounts_ShopperProductColors_ShopperProductColorId",
                table: "ShopperProductDiscounts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopperProductRequests_Colors_ColorId",
                table: "ShopperProductRequests");

            migrationBuilder.DropTable(
                name: "ShopperProductColors");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropIndex(
                name: "IX_ShopperProductRequests_ColorId",
                table: "ShopperProductRequests");

            migrationBuilder.DropIndex(
                name: "IX_ShopperProductDiscounts_ShopperProductColorId",
                table: "ShopperProductDiscounts");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ShopperProductColorId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteProducts_ShopperProductColorId",
                table: "FavoriteProducts");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "ShopperProductRequests");

            migrationBuilder.DropColumn(
                name: "ShopperProductColorId",
                table: "ShopperProductDiscounts");

            migrationBuilder.DropColumn(
                name: "ShopperProductColorId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ShopperProductColorId",
                table: "FavoriteProducts");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "ShopperProducts",
                newName: "IsInDiscount");

            migrationBuilder.AddColumn<byte>(
                name: "DiscountPercent",
                table: "ShopperProducts",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "ShopperProducts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ShopperProducts",
                type: "Money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "QuantityInStock",
                table: "ShopperProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SaleCount",
                table: "ShopperProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "ShopperProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CommentAnswers",
                columns: table => new
                {
                    CommentAnswerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerText = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CommentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentAnswers", x => x.CommentAnswerId);
                    table.ForeignKey(
                        name: "FK_CommentAnswers_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentAnswers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentAnswers_CommentId",
                table: "CommentAnswers",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentAnswers_UserId",
                table: "CommentAnswers",
                column: "UserId");
        }
    }
}
