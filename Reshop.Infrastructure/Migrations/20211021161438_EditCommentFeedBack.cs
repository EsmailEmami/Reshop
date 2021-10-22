using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class EditCommentFeedBack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentFeedBacks_Users_UserId",
                table: "CommentFeedBacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentFeedBacks",
                table: "CommentFeedBacks");

            migrationBuilder.DropIndex(
                name: "IX_CommentFeedBacks_CommentId",
                table: "CommentFeedBacks");

            migrationBuilder.DropColumn(
                name: "LikeCommentId",
                table: "CommentFeedBacks");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CommentFeedBacks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentFeedBacks",
                table: "CommentFeedBacks",
                columns: new[] { "CommentId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommentFeedBacks_Users_UserId",
                table: "CommentFeedBacks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentFeedBacks_Users_UserId",
                table: "CommentFeedBacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentFeedBacks",
                table: "CommentFeedBacks");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CommentFeedBacks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "LikeCommentId",
                table: "CommentFeedBacks",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentFeedBacks",
                table: "CommentFeedBacks",
                column: "LikeCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentFeedBacks_CommentId",
                table: "CommentFeedBacks",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentFeedBacks_Users_UserId",
                table: "CommentFeedBacks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
