using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class editBrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreTitleId",
                table: "Brands",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_StoreTitleId",
                table: "Brands",
                column: "StoreTitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_StoreTitles_StoreTitleId",
                table: "Brands",
                column: "StoreTitleId",
                principalTable: "StoreTitles",
                principalColumn: "StoreTitleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_StoreTitles_StoreTitleId",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Brands_StoreTitleId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "StoreTitleId",
                table: "Brands");
        }
    }
}
