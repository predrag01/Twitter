using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class m3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowingList_Users_FollowedId",
                table: "FollowingList");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowingList_Users_UserId",
                table: "FollowingList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FollowingList",
                table: "FollowingList");

            migrationBuilder.RenameTable(
                name: "FollowingList",
                newName: "Followings");

            migrationBuilder.RenameIndex(
                name: "IX_FollowingList_UserId",
                table: "Followings",
                newName: "IX_Followings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FollowingList_FollowedId",
                table: "Followings",
                newName: "IX_Followings_FollowedId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Followings",
                table: "Followings",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Followings_Users_FollowedId",
                table: "Followings",
                column: "FollowedId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Followings_Users_UserId",
                table: "Followings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followings_Users_FollowedId",
                table: "Followings");

            migrationBuilder.DropForeignKey(
                name: "FK_Followings_Users_UserId",
                table: "Followings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Followings",
                table: "Followings");

            migrationBuilder.RenameTable(
                name: "Followings",
                newName: "FollowingList");

            migrationBuilder.RenameIndex(
                name: "IX_Followings_UserId",
                table: "FollowingList",
                newName: "IX_FollowingList_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Followings_FollowedId",
                table: "FollowingList",
                newName: "IX_FollowingList_FollowedId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FollowingList",
                table: "FollowingList",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowingList_Users_FollowedId",
                table: "FollowingList",
                column: "FollowedId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowingList_Users_UserId",
                table: "FollowingList",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
