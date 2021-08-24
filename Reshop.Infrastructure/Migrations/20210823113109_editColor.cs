using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class editColor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortKey",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "ShortKey",
                table: "ShopperProductColors",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                table: "Colors",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortKey",
                table: "ShopperProductColors");

            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "Colors");

            migrationBuilder.AddColumn<string>(
                name: "ShortKey",
                table: "Products",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");
        }
    }
}
