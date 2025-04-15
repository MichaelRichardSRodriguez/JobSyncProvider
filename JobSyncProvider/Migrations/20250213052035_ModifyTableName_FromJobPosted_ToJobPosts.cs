using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSyncProvider.Migrations
{
    /// <inheritdoc />
    public partial class ModifyTableName_FromJobPosted_ToJobPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_JobPosted",
                table: "JobPosted");

            migrationBuilder.RenameTable(
                name: "JobPosted",
                newName: "JobPosts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobPosts",
                table: "JobPosts",
                column: "JobPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_JobPosts",
                table: "JobPosts");

            migrationBuilder.RenameTable(
                name: "JobPosts",
                newName: "JobPosted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobPosted",
                table: "JobPosted",
                column: "JobPostId");
        }
    }
}
