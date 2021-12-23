using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class addProductPermissionsDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "ParentId", "PermissionTitle" },
                values: new object[] { "fa5205e1-7395-4c3e-a464-72e84d38975a", null, "ProductsMainPage" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "02b75aeb-f9a1-4dbc-bf69-4c65cc29ec31",
                column: "Password",
                value: "7F-61-E7-33-23-CD-42-39-B9-38-18-F9-E5-46-23-91");

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "ParentId", "PermissionTitle" },
                values: new object[] { "4f27c20b-e51a-4152-9e7d-a5775ab969c6", "fa5205e1-7395-4c3e-a464-72e84d38975a", "AddAUX" });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "ParentId", "PermissionTitle" },
                values: new object[] { "5feb5422-b00a-47cd-b688-00ab6978441d", "fa5205e1-7395-4c3e-a464-72e84d38975a", "EditAUX" });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "ParentId", "PermissionTitle" },
                values: new object[] { "d62a9faf-087d-43a3-9bbe-66ae2737a0a5", "fa5205e1-7395-4c3e-a464-72e84d38975a", "ProductDetail" });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "ParentId", "PermissionTitle" },
                values: new object[] { "8db60935-056d-45a5-8044-a2e6e42e3edf", "d62a9faf-087d-43a3-9bbe-66ae2737a0a5", "ColorDetail-Product" });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "ParentId", "PermissionTitle" },
                values: new object[] { "91806aaf-6e67-4b3a-9554-faac7a5454f3", "d62a9faf-087d-43a3-9bbe-66ae2737a0a5", "DiscountDetail-Product" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "4f27c20b-e51a-4152-9e7d-a5775ab969c6");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "5feb5422-b00a-47cd-b688-00ab6978441d");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "8db60935-056d-45a5-8044-a2e6e42e3edf");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "91806aaf-6e67-4b3a-9554-faac7a5454f3");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "d62a9faf-087d-43a3-9bbe-66ae2737a0a5");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "fa5205e1-7395-4c3e-a464-72e84d38975a");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "02b75aeb-f9a1-4dbc-bf69-4c65cc29ec31",
                column: "Password",
                value: "1B-BD-88-64-60-82-70-15-E5-D6-05-ED-44-25-22-51");
        }
    }
}
