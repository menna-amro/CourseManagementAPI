using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class FinalRelationshipsSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Bio",
                table: "InstructorProfiles",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "InstructorProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "InstructorProfiles");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "InstructorProfiles",
                newName: "Bio");
        }
    }
}
