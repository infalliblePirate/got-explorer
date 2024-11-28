using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GotExplorer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class default_image_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "Name", "Path" },
                values: new object[] { 1, "", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
