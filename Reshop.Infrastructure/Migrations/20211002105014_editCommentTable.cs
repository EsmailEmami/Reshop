using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class editCommentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShopperProductColorId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ShopperProductColorId",
                table: "Comments",
                column: "ShopperProductColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ShopperProductColors_ShopperProductColorId",
                table: "Comments",
                column: "ShopperProductColorId",
                principalTable: "ShopperProductColors",
                principalColumn: "ShopperProductColorId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ShopperProductColors_ShopperProductColorId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ShopperProductColorId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ShopperProductColorId",
                table: "Comments");
        }
    }
}
