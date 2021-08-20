using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class editShoperProductColorRequestMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopperProductColorRequests_ShopperProductColors_ShopperProductColorId",
                table: "ShopperProductColorRequests");

            migrationBuilder.RenameColumn(
                name: "ShopperProductColorId",
                table: "ShopperProductColorRequests",
                newName: "ShopperProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopperProductColorRequests_ShopperProductColorId",
                table: "ShopperProductColorRequests",
                newName: "IX_ShopperProductColorRequests_ShopperProductId");

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "ShopperProductColorRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductColorRequests_ColorId",
                table: "ShopperProductColorRequests",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopperProductColorRequests_Colors_ColorId",
                table: "ShopperProductColorRequests",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "ColorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopperProductColorRequests_ShopperProducts_ShopperProductId",
                table: "ShopperProductColorRequests",
                column: "ShopperProductId",
                principalTable: "ShopperProducts",
                principalColumn: "ShopperProductId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopperProductColorRequests_Colors_ColorId",
                table: "ShopperProductColorRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopperProductColorRequests_ShopperProducts_ShopperProductId",
                table: "ShopperProductColorRequests");

            migrationBuilder.DropIndex(
                name: "IX_ShopperProductColorRequests_ColorId",
                table: "ShopperProductColorRequests");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "ShopperProductColorRequests");

            migrationBuilder.RenameColumn(
                name: "ShopperProductId",
                table: "ShopperProductColorRequests",
                newName: "ShopperProductColorId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopperProductColorRequests_ShopperProductId",
                table: "ShopperProductColorRequests",
                newName: "IX_ShopperProductColorRequests_ShopperProductColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopperProductColorRequests_ShopperProductColors_ShopperProductColorId",
                table: "ShopperProductColorRequests",
                column: "ShopperProductColorId",
                principalTable: "ShopperProductColors",
                principalColumn: "ShopperProductColorId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
