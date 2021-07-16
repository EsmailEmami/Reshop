using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class EditOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProducts_Shoppers_ShopperId",
                table: "FavoriteProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopperProducts_Shoppers_ShopperId",
                table: "ShopperProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopperProducts",
                table: "ShopperProducts");

            migrationBuilder.RenameColumn(
                name: "WatchForm",
                table: "WristWatchDetails",
                newName: "TypeOfLock");

            migrationBuilder.RenameColumn(
                name: "IsTouchScreen",
                table: "WristWatchDetails",
                newName: "TouchDisplay");

            migrationBuilder.RenameColumn(
                name: "IsSupportGPS",
                table: "WristWatchDetails",
                newName: "GPS");

            migrationBuilder.RenameColumn(
                name: "Access",
                table: "Products",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "ProductDiscount",
                table: "OrderDetails",
                newName: "ProductDiscountPrice");

            migrationBuilder.RenameColumn(
                name: "ShopperId",
                table: "FavoriteProducts",
                newName: "ShopperProductId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteProducts_ShopperId",
                table: "FavoriteProducts",
                newName: "IX_FavoriteProducts_ShopperProductId");

            migrationBuilder.AddColumn<string>(
                name: "Application",
                table: "WristWatchDetails",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BatteryCapacity",
                table: "WristWatchDetails",
                type: "int",
                maxLength: 5,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BatteryMaterial",
                table: "WristWatchDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BattryChargingS",
                table: "WristWatchDetails",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Compatibility",
                table: "WristWatchDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Connection",
                table: "WristWatchDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DisplayForm",
                table: "WristWatchDetails",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "DisplaySize",
                table: "WristWatchDetails",
                type: "float",
                maxLength: 8,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "DisplayType",
                table: "WristWatchDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Features",
                table: "WristWatchDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "WristWatchDetails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Lenght",
                table: "WristWatchDetails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaterialStrap",
                table: "WristWatchDetails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MoreInformation",
                table: "WristWatchDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PixelDensity",
                table: "WristWatchDetails",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Prossesor",
                table: "WristWatchDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Resists",
                table: "WristWatchDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Resolution",
                table: "WristWatchDetails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sensors",
                table: "WristWatchDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SuitableFor",
                table: "WristWatchDetails",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "WristWatchDetails",
                type: "int",
                maxLength: 20,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Width",
                table: "WristWatchDetails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ShopperId",
                table: "ShopperProducts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ShopperProductId",
                table: "ShopperProducts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsInDiscount",
                table: "ShopperProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "ShopperProducts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopperProducts",
                table: "ShopperProducts",
                column: "ShopperProductId");

            migrationBuilder.CreateTable(
                name: "ShopperProductDiscounts",
                columns: table => new
                {
                    ShopperProductDiscountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopperProductId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DiscountPercent = table.Column<byte>(type: "tinyint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopperProductDiscounts", x => x.ShopperProductDiscountId);
                    table.ForeignKey(
                        name: "FK_ShopperProductDiscounts_ShopperProducts_ShopperProductId",
                        column: x => x.ShopperProductId,
                        principalTable: "ShopperProducts",
                        principalColumn: "ShopperProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProducts_ShopperId",
                table: "ShopperProducts",
                column: "ShopperId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopperProductDiscounts_ShopperProductId",
                table: "ShopperProductDiscounts",
                column: "ShopperProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProducts_ShopperProducts_ShopperProductId",
                table: "FavoriteProducts",
                column: "ShopperProductId",
                principalTable: "ShopperProducts",
                principalColumn: "ShopperProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopperProducts_Shoppers_ShopperId",
                table: "ShopperProducts",
                column: "ShopperId",
                principalTable: "Shoppers",
                principalColumn: "ShopperId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProducts_ShopperProducts_ShopperProductId",
                table: "FavoriteProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopperProducts_Shoppers_ShopperId",
                table: "ShopperProducts");

            migrationBuilder.DropTable(
                name: "ShopperProductDiscounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopperProducts",
                table: "ShopperProducts");

            migrationBuilder.DropIndex(
                name: "IX_ShopperProducts_ShopperId",
                table: "ShopperProducts");

            migrationBuilder.DropColumn(
                name: "Application",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "BatteryCapacity",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "BatteryMaterial",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "BattryChargingS",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "Compatibility",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "Connection",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "DisplayForm",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "DisplaySize",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "DisplayType",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "Features",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "Lenght",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "MaterialStrap",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "MoreInformation",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "PixelDensity",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "Prossesor",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "Resists",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "Resolution",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "Sensors",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "SuitableFor",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "WristWatchDetails");

            migrationBuilder.DropColumn(
                name: "ShopperProductId",
                table: "ShopperProducts");

            migrationBuilder.DropColumn(
                name: "IsInDiscount",
                table: "ShopperProducts");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "ShopperProducts");

            migrationBuilder.RenameColumn(
                name: "TypeOfLock",
                table: "WristWatchDetails",
                newName: "WatchForm");

            migrationBuilder.RenameColumn(
                name: "TouchDisplay",
                table: "WristWatchDetails",
                newName: "IsTouchScreen");

            migrationBuilder.RenameColumn(
                name: "GPS",
                table: "WristWatchDetails",
                newName: "IsSupportGPS");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Products",
                newName: "Access");

            migrationBuilder.RenameColumn(
                name: "ProductDiscountPrice",
                table: "OrderDetails",
                newName: "ProductDiscount");

            migrationBuilder.RenameColumn(
                name: "ShopperProductId",
                table: "FavoriteProducts",
                newName: "ShopperId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteProducts_ShopperProductId",
                table: "FavoriteProducts",
                newName: "IX_FavoriteProducts_ShopperId");

            migrationBuilder.AlterColumn<string>(
                name: "ShopperId",
                table: "ShopperProducts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopperProducts",
                table: "ShopperProducts",
                columns: new[] { "ShopperId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProducts_Shoppers_ShopperId",
                table: "FavoriteProducts",
                column: "ShopperId",
                principalTable: "Shoppers",
                principalColumn: "ShopperId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopperProducts_Shoppers_ShopperId",
                table: "ShopperProducts",
                column: "ShopperId",
                principalTable: "Shoppers",
                principalColumn: "ShopperId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
