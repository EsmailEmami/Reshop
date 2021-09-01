using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class esitShopper2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Shoppers_ShopperId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ShopperId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsUserShopper",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ShopperId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IsFinally",
                table: "Shoppers",
                newName: "IsActive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Shoppers",
                newName: "IsFinally");

            migrationBuilder.AddColumn<bool>(
                name: "IsUserShopper",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShopperId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ShopperId",
                table: "Users",
                column: "ShopperId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Shoppers_ShopperId",
                table: "Users",
                column: "ShopperId",
                principalTable: "Shoppers",
                principalColumn: "ShopperId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
