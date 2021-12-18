using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class addSomeDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[] { "32757e0d-0c77-4ecd-bf82-6888acff29f1", "e9d0b742-79ff-4439-985e-bba8ae0d214d" });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[] { "3a86d2a6-8582-40c9-9c70-7b8c0efac6c1", "5fd1d3e0-b54c-4ea1-9762-80c6483fd3f8" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "3a86d2a6-8582-40c9-9c70-7b8c0efac6c1", "5fd1d3e0-b54c-4ea1-9762-80c6483fd3f8" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { "32757e0d-0c77-4ecd-bf82-6888acff29f1", "e9d0b742-79ff-4439-985e-bba8ae0d214d" });
        }
    }
}
