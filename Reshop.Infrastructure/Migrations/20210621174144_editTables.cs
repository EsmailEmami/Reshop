using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class editTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "ShopperProducts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsFinally",
                table: "ShopperProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ShopperProducts",
                type: "Money",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "ShopperProducts");

            migrationBuilder.DropColumn(
                name: "IsFinally",
                table: "ShopperProducts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ShopperProducts");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "Money",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
