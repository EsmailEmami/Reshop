using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class fullEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopperProductRequests_ShopperProducts_ShopperProductId",
                table: "ShopperProductRequests");

            migrationBuilder.DropIndex(
                name: "IX_ShopperProductRequests_ShopperProductId",
                table: "ShopperProductRequests");

            migrationBuilder.DropColumn(
                name: "ShopperProductId",
                table: "ShopperProductRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShopperProductId",
                table: "ShopperProductRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductRequests_ShopperProductId",
                table: "ShopperProductRequests",
                column: "ShopperProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopperProductRequests_ShopperProducts_ShopperProductId",
                table: "ShopperProductRequests",
                column: "ShopperProductId",
                principalTable: "ShopperProducts",
                principalColumn: "ShopperProductId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
