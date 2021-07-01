using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class EditShopperTBL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "StoresAddress");

            migrationBuilder.DropColumn(
                name: "State",
                table: "StoresAddress");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "StoresAddress",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "StoresAddress",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StoresAddress_CityId",
                table: "StoresAddress",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_StoresAddress_StateId",
                table: "StoresAddress",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoresAddress_Cities_CityId",
                table: "StoresAddress",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "CityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoresAddress_States_StateId",
                table: "StoresAddress",
                column: "StateId",
                principalTable: "States",
                principalColumn: "StateId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoresAddress_Cities_CityId",
                table: "StoresAddress");

            migrationBuilder.DropForeignKey(
                name: "FK_StoresAddress_States_StateId",
                table: "StoresAddress");

            migrationBuilder.DropIndex(
                name: "IX_StoresAddress_CityId",
                table: "StoresAddress");

            migrationBuilder.DropIndex(
                name: "IX_StoresAddress_StateId",
                table: "StoresAddress");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "StoresAddress");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "StoresAddress");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "StoresAddress",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "StoresAddress",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
