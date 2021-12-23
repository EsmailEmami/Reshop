using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class addSomeDataSeedsForRolePermission2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleTitle" },
                values: new object[,]
                {
                    { "e54362db-747b-4b70-bc1f-25542d836e48", "Roles Manager" },
                    { "84445749-e035-4bc1-9de5-146980b480a4", "Add Role" },
                    { "c5bf460c-7906-4fd2-a33c-ebddd8afa18d", "Edit Role" },
                    { "901ad48e-244f-4077-86fe-3d76e5495501", "Delete Role" },
                    { "64072c6a-f55b-4cfc-a1cf-8f2a98686c36", "Full Role Manager" },
                    { "6193141a-b2c6-4577-b1de-645f0425b8ae", "Permissions Manager" },
                    { "5df4eefa-87e9-4bf4-bbd0-8c90ab66e9b3", "Add Permission" },
                    { "baa40a2f-6e8e-468e-84f6-16c22865231e", "Edit Permission" },
                    { "c3884210-cedf-4cda-8547-ab6cb30731e7", "Delete Permission" },
                    { "211207c1-9669-4099-8955-90cba0639484", "Full Permission Manager" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { "ec223189-ef42-41bc-b01f-6d58db051e62", "64072c6a-f55b-4cfc-a1cf-8f2a98686c36" },
                    { "00dd702b-1e83-4338-ae4b-3f4db4244aa5", "64072c6a-f55b-4cfc-a1cf-8f2a98686c36" },
                    { "404efa94-c45f-4c50-ad3d-faaaf6b61807", "64072c6a-f55b-4cfc-a1cf-8f2a98686c36" },
                    { "c154f3e4-c485-47b2-8957-6d7b177ce466", "64072c6a-f55b-4cfc-a1cf-8f2a98686c36" },
                    { "bef2eb25-1007-40ae-b667-dcff4c4a07a9", "211207c1-9669-4099-8955-90cba0639484" },
                    { "afa3e6c4-cd57-4bed-ba82-c656bed921ff", "211207c1-9669-4099-8955-90cba0639484" },
                    { "949826a7-070f-450a-abd8-effca32350c2", "211207c1-9669-4099-8955-90cba0639484" },
                    { "4c516b2a-0149-4626-8d19-c2d6ac51a425", "211207c1-9669-4099-8955-90cba0639484" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "4c516b2a-0149-4626-8d19-c2d6ac51a425", "211207c1-9669-4099-8955-90cba0639484" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "949826a7-070f-450a-abd8-effca32350c2", "211207c1-9669-4099-8955-90cba0639484" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "afa3e6c4-cd57-4bed-ba82-c656bed921ff", "211207c1-9669-4099-8955-90cba0639484" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "bef2eb25-1007-40ae-b667-dcff4c4a07a9", "211207c1-9669-4099-8955-90cba0639484" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "00dd702b-1e83-4338-ae4b-3f4db4244aa5", "64072c6a-f55b-4cfc-a1cf-8f2a98686c36" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "404efa94-c45f-4c50-ad3d-faaaf6b61807", "64072c6a-f55b-4cfc-a1cf-8f2a98686c36" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "c154f3e4-c485-47b2-8957-6d7b177ce466", "64072c6a-f55b-4cfc-a1cf-8f2a98686c36" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "ec223189-ef42-41bc-b01f-6d58db051e62", "64072c6a-f55b-4cfc-a1cf-8f2a98686c36" });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "5df4eefa-87e9-4bf4-bbd0-8c90ab66e9b3");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "6193141a-b2c6-4577-b1de-645f0425b8ae");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "84445749-e035-4bc1-9de5-146980b480a4");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "901ad48e-244f-4077-86fe-3d76e5495501");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "baa40a2f-6e8e-468e-84f6-16c22865231e");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "c3884210-cedf-4cda-8547-ab6cb30731e7");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "c5bf460c-7906-4fd2-a33c-ebddd8afa18d");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "e54362db-747b-4b70-bc1f-25542d836e48");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "211207c1-9669-4099-8955-90cba0639484");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: "64072c6a-f55b-4cfc-a1cf-8f2a98686c36");
        }
    }
}
