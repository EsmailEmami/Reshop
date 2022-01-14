using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reshop.Infrastructure.Migrations
{
    public partial class EditMobileProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CameraQuantity",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "ChipsetName",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "Cpu",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "CpuArch",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "Filming",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "Gpu",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "Lenght",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "OsVersion",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "PhotoResolutation",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "SelfiCameraPhoto",
                table: "MobileDetails");

            migrationBuilder.RenameColumn(
                name: "SimCardInpute",
                table: "MobileDetails",
                newName: "SimCardInput");

            migrationBuilder.RenameColumn(
                name: "OS",
                table: "MobileDetails",
                newName: "Length");

            migrationBuilder.RenameColumn(
                name: "MoreInformation",
                table: "MobileDetails",
                newName: "ConnectionsMoreInformation");

            migrationBuilder.RenameColumn(
                name: "AudioInformation",
                table: "MobileDetails",
                newName: "AudioMoreInformation");

            migrationBuilder.AlterColumn<string>(
                name: "Sensors",
                table: "MobileDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SelfiCameraCapabilities",
                table: "MobileDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Radio",
                table: "MobileDetails",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "MoreInformationSoftWare",
                table: "MobileDetails",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ItemsInBox",
                table: "MobileDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "GsmNetwork",
                table: "MobileDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CameraCapabilities",
                table: "MobileDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Announced",
                table: "MobileDetails",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "CameraMoreInformation",
                table: "MobileDetails",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cameras",
                table: "MobileDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChipsetId",
                table: "MobileDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CpuArchId",
                table: "MobileDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CpuId",
                table: "MobileDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepthCameraCapabilities",
                table: "MobileDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepthCameraResolution",
                table: "MobileDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DepthCameraVideo",
                table: "MobileDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayMoreInformation",
                table: "MobileDetails",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GpuId",
                table: "MobileDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MacroCameraCapabilities",
                table: "MobileDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MacroCameraResolution",
                table: "MobileDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MacroCameraVideo",
                table: "MobileDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperatingSystemId",
                table: "MobileDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperatingSystemVersionId",
                table: "MobileDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoCameraVideo",
                table: "MobileDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhotoResolution",
                table: "MobileDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SelfiCameraResolution",
                table: "MobileDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SelfiCameraVideo",
                table: "MobileDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WideCameraCapabilities",
                table: "MobileDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WideCameraResolution",
                table: "MobileDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WideCameraVideo",
                table: "MobileDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Chipsets",
                columns: table => new
                {
                    ChipsetId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChipsetName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chipsets", x => x.ChipsetId);
                });

            migrationBuilder.CreateTable(
                name: "CpuArches",
                columns: table => new
                {
                    CpuArchId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CpuArchName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpuArches", x => x.CpuArchId);
                });

            migrationBuilder.CreateTable(
                name: "OperatingSystems",
                columns: table => new
                {
                    OperatingSystemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OperatingSystemName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatingSystems", x => x.OperatingSystemId);
                });

            migrationBuilder.CreateTable(
                name: "Cpus",
                columns: table => new
                {
                    CpuId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CpuName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ChipsetId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cpus", x => x.CpuId);
                    table.ForeignKey(
                        name: "FK_Cpus_Chipsets_ChipsetId",
                        column: x => x.ChipsetId,
                        principalTable: "Chipsets",
                        principalColumn: "ChipsetId");
                });

            migrationBuilder.CreateTable(
                name: "Gpus",
                columns: table => new
                {
                    GpuId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GpuName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ChipsetId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gpus", x => x.GpuId);
                    table.ForeignKey(
                        name: "FK_Gpus_Chipsets_ChipsetId",
                        column: x => x.ChipsetId,
                        principalTable: "Chipsets",
                        principalColumn: "ChipsetId");
                });

            migrationBuilder.CreateTable(
                name: "OperatingSystemVersions",
                columns: table => new
                {
                    OperatingSystemVersionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OperatingSystemVersionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OperatingSystemId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatingSystemVersions", x => x.OperatingSystemVersionId);
                    table.ForeignKey(
                        name: "FK_OperatingSystemVersions_OperatingSystems_OperatingSystemId",
                        column: x => x.OperatingSystemId,
                        principalTable: "OperatingSystems",
                        principalColumn: "OperatingSystemId");
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "02b75aeb-f9a1-4dbc-bf69-4c65cc29ec31",
                column: "Password",
                value: "a?3#?B9?8??F#?");

            migrationBuilder.CreateIndex(
                name: "IX_MobileDetails_ChipsetId",
                table: "MobileDetails",
                column: "ChipsetId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileDetails_CpuArchId",
                table: "MobileDetails",
                column: "CpuArchId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileDetails_CpuId",
                table: "MobileDetails",
                column: "CpuId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileDetails_GpuId",
                table: "MobileDetails",
                column: "GpuId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileDetails_OperatingSystemId",
                table: "MobileDetails",
                column: "OperatingSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileDetails_OperatingSystemVersionId",
                table: "MobileDetails",
                column: "OperatingSystemVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Cpus_ChipsetId",
                table: "Cpus",
                column: "ChipsetId");

            migrationBuilder.CreateIndex(
                name: "IX_Gpus_ChipsetId",
                table: "Gpus",
                column: "ChipsetId");

            migrationBuilder.CreateIndex(
                name: "IX_OperatingSystemVersions_OperatingSystemId",
                table: "OperatingSystemVersions",
                column: "OperatingSystemId");

            migrationBuilder.AddForeignKey(
                name: "FK_MobileDetails_Chipsets_ChipsetId",
                table: "MobileDetails",
                column: "ChipsetId",
                principalTable: "Chipsets",
                principalColumn: "ChipsetId");

            migrationBuilder.AddForeignKey(
                name: "FK_MobileDetails_CpuArches_CpuArchId",
                table: "MobileDetails",
                column: "CpuArchId",
                principalTable: "CpuArches",
                principalColumn: "CpuArchId");

            migrationBuilder.AddForeignKey(
                name: "FK_MobileDetails_Cpus_CpuId",
                table: "MobileDetails",
                column: "CpuId",
                principalTable: "Cpus",
                principalColumn: "CpuId");

            migrationBuilder.AddForeignKey(
                name: "FK_MobileDetails_Gpus_GpuId",
                table: "MobileDetails",
                column: "GpuId",
                principalTable: "Gpus",
                principalColumn: "GpuId");

            migrationBuilder.AddForeignKey(
                name: "FK_MobileDetails_OperatingSystems_OperatingSystemId",
                table: "MobileDetails",
                column: "OperatingSystemId",
                principalTable: "OperatingSystems",
                principalColumn: "OperatingSystemId");

            migrationBuilder.AddForeignKey(
                name: "FK_MobileDetails_OperatingSystemVersions_OperatingSystemVersionId",
                table: "MobileDetails",
                column: "OperatingSystemVersionId",
                principalTable: "OperatingSystemVersions",
                principalColumn: "OperatingSystemVersionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MobileDetails_Chipsets_ChipsetId",
                table: "MobileDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MobileDetails_CpuArches_CpuArchId",
                table: "MobileDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MobileDetails_Cpus_CpuId",
                table: "MobileDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MobileDetails_Gpus_GpuId",
                table: "MobileDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MobileDetails_OperatingSystems_OperatingSystemId",
                table: "MobileDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MobileDetails_OperatingSystemVersions_OperatingSystemVersionId",
                table: "MobileDetails");

            migrationBuilder.DropTable(
                name: "CpuArches");

            migrationBuilder.DropTable(
                name: "Cpus");

            migrationBuilder.DropTable(
                name: "Gpus");

            migrationBuilder.DropTable(
                name: "OperatingSystemVersions");

            migrationBuilder.DropTable(
                name: "Chipsets");

            migrationBuilder.DropTable(
                name: "OperatingSystems");

            migrationBuilder.DropIndex(
                name: "IX_MobileDetails_ChipsetId",
                table: "MobileDetails");

            migrationBuilder.DropIndex(
                name: "IX_MobileDetails_CpuArchId",
                table: "MobileDetails");

            migrationBuilder.DropIndex(
                name: "IX_MobileDetails_CpuId",
                table: "MobileDetails");

            migrationBuilder.DropIndex(
                name: "IX_MobileDetails_GpuId",
                table: "MobileDetails");

            migrationBuilder.DropIndex(
                name: "IX_MobileDetails_OperatingSystemId",
                table: "MobileDetails");

            migrationBuilder.DropIndex(
                name: "IX_MobileDetails_OperatingSystemVersionId",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "CameraMoreInformation",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "Cameras",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "ChipsetId",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "CpuArchId",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "CpuId",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "DepthCameraCapabilities",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "DepthCameraResolution",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "DepthCameraVideo",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "DisplayMoreInformation",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "GpuId",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "MacroCameraCapabilities",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "MacroCameraResolution",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "MacroCameraVideo",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "OperatingSystemId",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "OperatingSystemVersionId",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "PhotoCameraVideo",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "PhotoResolution",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "SelfiCameraResolution",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "SelfiCameraVideo",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "WideCameraCapabilities",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "WideCameraResolution",
                table: "MobileDetails");

            migrationBuilder.DropColumn(
                name: "WideCameraVideo",
                table: "MobileDetails");

            migrationBuilder.RenameColumn(
                name: "SimCardInput",
                table: "MobileDetails",
                newName: "SimCardInpute");

            migrationBuilder.RenameColumn(
                name: "Length",
                table: "MobileDetails",
                newName: "OS");

            migrationBuilder.RenameColumn(
                name: "ConnectionsMoreInformation",
                table: "MobileDetails",
                newName: "MoreInformation");

            migrationBuilder.RenameColumn(
                name: "AudioMoreInformation",
                table: "MobileDetails",
                newName: "AudioInformation");

            migrationBuilder.AlterColumn<string>(
                name: "Sensors",
                table: "MobileDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "SelfiCameraCapabilities",
                table: "MobileDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Radio",
                table: "MobileDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "MoreInformationSoftWare",
                table: "MobileDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "ItemsInBox",
                table: "MobileDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "GsmNetwork",
                table: "MobileDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "CameraCapabilities",
                table: "MobileDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Announced",
                table: "MobileDetails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "CameraQuantity",
                table: "MobileDetails",
                type: "int",
                maxLength: 2,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ChipsetName",
                table: "MobileDetails",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cpu",
                table: "MobileDetails",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CpuArch",
                table: "MobileDetails",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Filming",
                table: "MobileDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Gpu",
                table: "MobileDetails",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Lenght",
                table: "MobileDetails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OsVersion",
                table: "MobileDetails",
                type: "int",
                maxLength: 3,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PhotoResolutation",
                table: "MobileDetails",
                type: "int",
                maxLength: 4,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SelfiCameraPhoto",
                table: "MobileDetails",
                type: "int",
                maxLength: 4,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "02b75aeb-f9a1-4dbc-bf69-4c65cc29ec31",
                column: "Password",
                value: "7F-61-E7-33-23-CD-42-39-B9-38-18-F9-E5-46-23-91");
        }
    }
}
