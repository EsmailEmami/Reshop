using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class editUser1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "02b75aeb-f9a1-4dbc-bf69-4c65cc29ec31",
                column: "Password",
                value: "1B-BD-88-64-60-82-70-15-E5-D6-05-ED-44-25-22-51");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");
        }
    }
}
