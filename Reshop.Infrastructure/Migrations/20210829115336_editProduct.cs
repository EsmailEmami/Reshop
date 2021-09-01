using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class editProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_OfficialBrandProducts_OfficialBrandProductId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrandId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductGalleries",
                table: "ProductGalleries");

            migrationBuilder.DropIndex(
                name: "IX_ProductGalleries_ProductId",
                table: "ProductGalleries");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductGalleryId",
                table: "ProductGalleries");

            migrationBuilder.AlterColumn<int>(
                name: "OfficialBrandProductId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductGalleries",
                table: "ProductGalleries",
                columns: new[] { "ProductId", "ImageName" });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_OfficialBrandProducts_OfficialBrandProductId",
                table: "Products",
                column: "OfficialBrandProductId",
                principalTable: "OfficialBrandProducts",
                principalColumn: "OfficialBrandProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_OfficialBrandProducts_OfficialBrandProductId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductGalleries",
                table: "ProductGalleries");

            migrationBuilder.AlterColumn<int>(
                name: "OfficialBrandProductId",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductGalleryId",
                table: "ProductGalleries",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductGalleries",
                table: "ProductGalleries",
                column: "ProductGalleryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGalleries_ProductId",
                table: "ProductGalleries",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_OfficialBrandProducts_OfficialBrandProductId",
                table: "Products",
                column: "OfficialBrandProductId",
                principalTable: "OfficialBrandProducts",
                principalColumn: "OfficialBrandProductId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
