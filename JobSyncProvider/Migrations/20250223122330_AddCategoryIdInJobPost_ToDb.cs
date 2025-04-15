using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSyncProvider.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryIdInJobPost_ToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPosts_Categories_CategoryId",
                table: "JobPosts");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "JobPosts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JobPosts_Categories_CategoryId",
                table: "JobPosts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPosts_Categories_CategoryId",
                table: "JobPosts");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "JobPosts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPosts_Categories_CategoryId",
                table: "JobPosts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");
        }
    }
}
