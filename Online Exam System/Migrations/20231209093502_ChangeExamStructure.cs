using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Exam_System.Migrations
{
    /// <inheritdoc />
    public partial class ChangeExamStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionPoints",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExamPoints",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SuccessDegree",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1436f2dc-bc6c-43ca-8497-feb0b0983066", "AQAAAAIAAYagAAAAEBjDCzhJp5AGP+l1LQC00WPnRaBgq6UZKtFSSGxAMqLMBDp86eVsHidsRoqS8IfMgg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionPoints",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ExamPoints",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "SuccessDegree",
                table: "Exams");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5097645b-d7ac-4a82-93f9-9a2a9355d306", "AQAAAAIAAYagAAAAEIS2/R7hI9GxSzOUiDGeBXvsYFhuTSVH+SqvtoJtZ+7IjNvKYPqvdYJu6abORzMofg==" });
        }
    }
}
