using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeLearn.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_LevelStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseLevels_CourseLevelLevelId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseStatuses_CourseStatusStatusId",
                table: "Courses");

            migrationBuilder.AlterColumn<int>(
                name: "CourseStatusStatusId",
                table: "Courses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CourseLevelLevelId",
                table: "Courses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseLevels_CourseLevelLevelId",
                table: "Courses",
                column: "CourseLevelLevelId",
                principalTable: "CourseLevels",
                principalColumn: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseStatuses_CourseStatusStatusId",
                table: "Courses",
                column: "CourseStatusStatusId",
                principalTable: "CourseStatuses",
                principalColumn: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseLevels_CourseLevelLevelId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseStatuses_CourseStatusStatusId",
                table: "Courses");

            migrationBuilder.AlterColumn<int>(
                name: "CourseStatusStatusId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CourseLevelLevelId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseLevels_CourseLevelLevelId",
                table: "Courses",
                column: "CourseLevelLevelId",
                principalTable: "CourseLevels",
                principalColumn: "LevelId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseStatuses_CourseStatusStatusId",
                table: "Courses",
                column: "CourseStatusStatusId",
                principalTable: "CourseStatuses",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
