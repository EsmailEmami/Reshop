using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class editCommentScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "ConstructionQuality",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DesignAndAppearance",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FeaturesAndCapabilities",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductSatisfaction",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConstructionQuality",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "DesignAndAppearance",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "FeaturesAndCapabilities",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ProductSatisfaction",
                table: "Comments");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
