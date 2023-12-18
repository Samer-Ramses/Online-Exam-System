using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Exam_System.Migrations
{
    /// <inheritdoc />
    public partial class userAnswerPKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAnswers",
                table: "UserAnswers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAnswers",
                table: "UserAnswers",
                columns: new[] { "AttemptID", "QuestionID" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "218cfcec-7eb7-487f-800f-bb72d98aa3d1", "AQAAAAIAAYagAAAAEPl1NSsjIM7Ngc03arSepg/d/0DPyACsylIqfXpUPuExrnXCvSNwQefcltQqcUwlnQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAnswers",
                table: "UserAnswers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAnswers",
                table: "UserAnswers",
                column: "AttemptID");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "61047cc4-b31d-4b00-949b-8f79172f9f0b", "AQAAAAIAAYagAAAAEICgrrOD9ewzJx65cA/Fw5456fvgYmZwLK75+0BGrWbV5vqoAUUM8Zn+0haYCL4u9A==" });
        }
    }
}
