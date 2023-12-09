using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Exam_System.Migrations
{
    /// <inheritdoc />
    public partial class addExamCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExamCode",
                table: "Exams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b99efbc9-536a-4256-9080-c86a37d94785", "AQAAAAIAAYagAAAAECMkxdUGiQUaB0JAbVmXzlmsc5kzRA9zgOvY7fDYAwCVx0Hv3jhXMUYovMT4UJxtWw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamCode",
                table: "Exams");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1436f2dc-bc6c-43ca-8497-feb0b0983066", "AQAAAAIAAYagAAAAEBjDCzhJp5AGP+l1LQC00WPnRaBgq6UZKtFSSGxAMqLMBDp86eVsHidsRoqS8IfMgg==" });
        }
    }
}
