using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class addSomeDataSeedsforRolePermission1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleTitle" },
                values: new object[,]
                {
                    { "cd2f26bd-0552-4fb0-be45-ca10741a7ab2", "Products Manager" },
                    { "36aaf88b-d399-467c-af03-4bc902b4ac6d", "Add Products" },
                    { "04d8adb3-f7cc-4595-b049-22d9e1fff55e", "Product Detail" },
                    { "4b8c123d-56c2-41d0-baf9-b7794031bf80", "Edit Products" },
                    { "2762ff27-40af-45a2-9a3a-41046597fce5", "Full Products Manager" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { "fa5205e1-7395-4c3e-a464-72e84d38975a", "2762ff27-40af-45a2-9a3a-41046597fce5" },
                    { "4f27c20b-e51a-4152-9e7d-a5775ab969c6", "2762ff27-40af-45a2-9a3a-41046597fce5" },
                    { "5feb5422-b00a-47cd-b688-00ab6978441d", "2762ff27-40af-45a2-9a3a-41046597fce5" },
                    { "d62a9faf-087d-43a3-9bbe-66ae2737a0a5", "2762ff27-40af-45a2-9a3a-41046597fce5" },
                    { "8db60935-056d-45a5-8044-a2e6e42e3edf", "2762ff27-40af-45a2-9a3a-41046597fce5" },
                    { "91806aaf-6e67-4b3a-9554-faac7a5454f3", "2762ff27-40af-45a2-9a3a-41046597fce5" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "4f27c20b-e51a-4152-9e7d-a5775ab969c6", "2762ff27-40af-45a2-9a3a-41046597fce5" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "5feb5422-b00a-47cd-b688-00ab6978441d", "2762ff27-40af-45a2-9a3a-41046597fce5" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "8db60935-056d-45a5-8044-a2e6e42e3edf", "2762ff27-40af-45a2-9a3a-41046597fce5" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "91806aaf-6e67-4b3a-9554-faac7a5454f3", "2762ff27-40af-45a2-9a3a-41046597fce5" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "d62a9faf-087d-43a3-9bbe-66ae2737a0a5", "2762ff27-40af-45a2-9a3a-41046597fce5" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "fa5205e1-7395-4c3e-a464-72e84d38975a", "2762ff27-40af-45a2-9a3a-41046597fce5" });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "04d8adb3-f7cc-4595-b049-22d9e1fff55e");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "36aaf88b-d399-467c-af03-4bc902b4ac6d");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "4b8c123d-56c2-41d0-baf9-b7794031bf80");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "cd2f26bd-0552-4fb0-be45-ca10741a7ab2");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "2762ff27-40af-45a2-9a3a-41046597fce5");
        }
    }
}
