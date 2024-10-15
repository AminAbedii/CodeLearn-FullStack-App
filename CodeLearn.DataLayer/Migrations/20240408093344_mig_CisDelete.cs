using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeLearn.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_CisDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Courses");
        }
    }
}
