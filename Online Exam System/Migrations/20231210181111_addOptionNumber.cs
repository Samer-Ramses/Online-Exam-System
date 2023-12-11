using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Exam_System.Migrations
{
    /// <inheritdoc />
    public partial class addOptionNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OptionsNumber",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "61047cc4-b31d-4b00-949b-8f79172f9f0b", "AQAAAAIAAYagAAAAEICgrrOD9ewzJx65cA/Fw5456fvgYmZwLK75+0BGrWbV5vqoAUUM8Zn+0haYCL4u9A==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptionsNumber",
                table: "Questions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b99efbc9-536a-4256-9080-c86a37d94785", "AQAAAAIAAYagAAAAECMkxdUGiQUaB0JAbVmXzlmsc5kzRA9zgOvY7fDYAwCVx0Hv3jhXMUYovMT4UJxtWw==" });
        }
    }
}
