using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Code_Orbit.Data.Migrations
{
    /// <inheritdoc />
    public partial class CoursesTableNewSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EnrollmentUrl",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Syllabus",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnrollmentUrl",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Syllabus",
                table: "Courses");
        }
    }
}
