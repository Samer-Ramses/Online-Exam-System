using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Exam_System.Migrations
{
    /// <inheritdoc />
    public partial class addPicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePictureData",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "ProfilePictureData" },
                values: new object[] { "5097645b-d7ac-4a82-93f9-9a2a9355d306", "AQAAAAIAAYagAAAAEIS2/R7hI9GxSzOUiDGeBXvsYFhuTSVH+SqvtoJtZ+7IjNvKYPqvdYJu6abORzMofg==", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureData",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "91fb266c-f351-4116-ab72-ce62f68c07b5", "AQAAAAIAAYagAAAAEAabJJEDPxPQA8f2O/EDBj8kuFIF71DCJy1SVBZv0B1RHQB1X79D0/QZt8BuFw2q9g==" });
        }
    }
}
