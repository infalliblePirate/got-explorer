using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GotExplorer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Addedrowversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Models3D",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Levels",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Images",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Models3D");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Images");
        }
    }
}
