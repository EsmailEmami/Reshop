using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class addBrandToChildCategoryTBL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrandToChildCategories",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    ChildCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandToChildCategories", x => new { x.BrandId, x.ChildCategoryId });
                    table.ForeignKey(
                        name: "FK_BrandToChildCategories_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandToChildCategories_ChildCategories_ChildCategoryId",
                        column: x => x.ChildCategoryId,
                        principalTable: "ChildCategories",
                        principalColumn: "ChildCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandToChildCategories_ChildCategoryId",
                table: "BrandToChildCategories",
                column: "ChildCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandToChildCategories");
        }
    }
}
