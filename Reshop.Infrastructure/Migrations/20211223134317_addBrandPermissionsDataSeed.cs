using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class addBrandPermissionsDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "ParentId", "PermissionTitle" },
                values: new object[] { "75a1d67a-8b13-472b-b93e-61186c41f5b2", null, "BrandsMainPage" });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "ParentId", "PermissionTitle" },
                values: new object[,]
                {
                    { "5a597555-a3ac-4c3a-8ab6-50961b43a4b6", "75a1d67a-8b13-472b-b93e-61186c41f5b2", "AddBrand" },
                    { "448e89b5-b5fe-4c65-8f73-49df8a749cc0", "75a1d67a-8b13-472b-b93e-61186c41f5b2", "EditBrand" },
                    { "d54aaed6-97f1-47fb-81ec-be6140c24f74", "75a1d67a-8b13-472b-b93e-61186c41f5b2", "BrandDetail" },
                    { "e87bb2b0-8c05-40ec-9b48-4008eeac79be", "75a1d67a-8b13-472b-b93e-61186c41f5b2", "AvailableBrand" },
                    { "c53de576-d7d5-4f62-9267-a88f17c9a952", "75a1d67a-8b13-472b-b93e-61186c41f5b2", "UnAvailableBrand" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "448e89b5-b5fe-4c65-8f73-49df8a749cc0");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "5a597555-a3ac-4c3a-8ab6-50961b43a4b6");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "c53de576-d7d5-4f62-9267-a88f17c9a952");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "d54aaed6-97f1-47fb-81ec-be6140c24f74");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "e87bb2b0-8c05-40ec-9b48-4008eeac79be");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "75a1d67a-8b13-472b-b93e-61186c41f5b2");
        }
    }
}
