using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSyncProvider.Migrations
{
    /// <inheritdoc />
    public partial class AddJobStatusInJobApplications_ToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "JobApplications",
                newName: "JobStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobStatus",
                table: "JobApplications",
                newName: "Status");
        }
    }
}
