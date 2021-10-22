using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class AddReportComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "Comments");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "ReportComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "ReportComments");

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
