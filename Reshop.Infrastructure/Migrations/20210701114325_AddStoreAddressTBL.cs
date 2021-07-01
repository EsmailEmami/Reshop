using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class AddStoreAddressTBL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Condition",
                table: "Shoppers");

            migrationBuilder.DropColumn(
                name: "LandlinePhoneNumber",
                table: "Shoppers");

            migrationBuilder.RenameColumn(
                name: "IsApproved",
                table: "Shoppers",
                newName: "IsFinally");

            migrationBuilder.CreateTable(
                name: "StoresAddress",
                columns: table => new
                {
                    StoreAddressId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopperId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    City = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Plaque = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AddressText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    LandlinePhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoresAddress", x => x.StoreAddressId);
                    table.ForeignKey(
                        name: "FK_StoresAddress_Shoppers_ShopperId",
                        column: x => x.ShopperId,
                        principalTable: "Shoppers",
                        principalColumn: "ShopperId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoresAddress_ShopperId",
                table: "StoresAddress",
                column: "ShopperId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoresAddress");

            migrationBuilder.RenameColumn(
                name: "IsFinally",
                table: "Shoppers",
                newName: "IsApproved");

            migrationBuilder.AddColumn<bool>(
                name: "Condition",
                table: "Shoppers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LandlinePhoneNumber",
                table: "Shoppers",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");
        }
    }
}
