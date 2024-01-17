using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class m4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followings_Users_UserId",
                table: "Followings");

            migrationBuilder.DropColumn(
                name: "FollowedCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FollowingCount",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Followings",
                newName: "FollowerId");

            migrationBuilder.RenameIndex(
                name: "IX_Followings_UserId",
                table: "Followings",
                newName: "IX_Followings_FollowerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Followings_Users_FollowerId",
                table: "Followings",
                column: "FollowerId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followings_Users_FollowerId",
                table: "Followings");

            migrationBuilder.RenameColumn(
                name: "FollowerId",
                table: "Followings",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Followings_FollowerId",
                table: "Followings",
                newName: "IX_Followings_UserId");

            migrationBuilder.AddColumn<int>(
                name: "FollowedCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FollowingCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Followings_Users_UserId",
                table: "Followings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
