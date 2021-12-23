using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class addSomeDataSeedsforPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "ParentId", "PermissionTitle" },
                values: new object[,]
                {
                    { "d08d44f8-9f67-4079-b1c7-8f5bc126b66b", null, "OfficialBrandProductsMainPage" },
                    { "8200832d-f2b6-4c7f-be15-b94592db8b0b", null, "ColorsMainPage" },
                    { "bef2eb25-1007-40ae-b667-dcff4c4a07a9", null, "PermissionsMainPage" },
                    { "ec223189-ef42-41bc-b01f-6d58db051e62", null, "RolesMainPage" }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "ParentId", "PermissionTitle" },
                values: new object[,]
                {
                    { "6d4e2f7b-a4bc-47b1-93ac-bcf9a0b3b3f0", "d08d44f8-9f67-4079-b1c7-8f5bc126b66b", "AddOfficialBrandProduct" },
                    { "526a98eb-79fd-4e54-bea8-ed7e85c0497f", "d08d44f8-9f67-4079-b1c7-8f5bc126b66b", "EditOfficialBrandProduct" },
                    { "28cf0756-89f5-4a16-a5f8-64adcb095bce", "d08d44f8-9f67-4079-b1c7-8f5bc126b66b", "OfficialBrandProductDetail" },
                    { "26eb15bc-b6c0-462e-8e7e-9652d56e549b", "d08d44f8-9f67-4079-b1c7-8f5bc126b66b", "AvailableOfficialBrandProduct" },
                    { "c06312fc-c36c-4e12-8964-5f893eeef3cf", "d08d44f8-9f67-4079-b1c7-8f5bc126b66b", "UnAvailableOfficialBrandProduct" },
                    { "2ce5f38d-6e9a-4de2-8fcf-dca1baf0902a", "8200832d-f2b6-4c7f-be15-b94592db8b0b", "AddColor" },
                    { "5660f404-46f3-42c9-818e-e5d6a8cc514b", "8200832d-f2b6-4c7f-be15-b94592db8b0b", "EditColor" },
                    { "afa3e6c4-cd57-4bed-ba82-c656bed921ff", "bef2eb25-1007-40ae-b667-dcff4c4a07a9", "AddPermission" },
                    { "949826a7-070f-450a-abd8-effca32350c2", "bef2eb25-1007-40ae-b667-dcff4c4a07a9", "EditPermission" },
                    { "4c516b2a-0149-4626-8d19-c2d6ac51a425", "bef2eb25-1007-40ae-b667-dcff4c4a07a9", "DeletePermission" },
                    { "00dd702b-1e83-4338-ae4b-3f4db4244aa5", "ec223189-ef42-41bc-b01f-6d58db051e62", "AddRole" },
                    { "404efa94-c45f-4c50-ad3d-faaaf6b61807", "ec223189-ef42-41bc-b01f-6d58db051e62", "EditRole" },
                    { "c154f3e4-c485-47b2-8957-6d7b177ce466", "ec223189-ef42-41bc-b01f-6d58db051e62", "DeleteRole" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "00dd702b-1e83-4338-ae4b-3f4db4244aa5");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "26eb15bc-b6c0-462e-8e7e-9652d56e549b");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "28cf0756-89f5-4a16-a5f8-64adcb095bce");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "2ce5f38d-6e9a-4de2-8fcf-dca1baf0902a");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "404efa94-c45f-4c50-ad3d-faaaf6b61807");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "4c516b2a-0149-4626-8d19-c2d6ac51a425");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "526a98eb-79fd-4e54-bea8-ed7e85c0497f");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "5660f404-46f3-42c9-818e-e5d6a8cc514b");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "6d4e2f7b-a4bc-47b1-93ac-bcf9a0b3b3f0");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "949826a7-070f-450a-abd8-effca32350c2");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "afa3e6c4-cd57-4bed-ba82-c656bed921ff");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "c06312fc-c36c-4e12-8964-5f893eeef3cf");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "c154f3e4-c485-47b2-8957-6d7b177ce466");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "8200832d-f2b6-4c7f-be15-b94592db8b0b");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "bef2eb25-1007-40ae-b667-dcff4c4a07a9");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "d08d44f8-9f67-4079-b1c7-8f5bc126b66b");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: "ec223189-ef42-41bc-b01f-6d58db051e62");
        }
    }
}
