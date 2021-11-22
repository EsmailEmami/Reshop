using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class editStoreAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoresAddress_States_StateId",
                table: "StoresAddress");

            migrationBuilder.DropIndex(
                name: "IX_StoresAddress_StateId",
                table: "StoresAddress");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "StoresAddress");

            migrationBuilder.AddColumn<string>(
                name: "StoreName",
                table: "StoresAddress",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoreName",
                table: "StoresAddress");

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "StoresAddress",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoresAddress_StateId",
                table: "StoresAddress",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoresAddress_States_StateId",
                table: "StoresAddress",
                column: "StateId",
                principalTable: "States",
                principalColumn: "StateId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
