using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GotExplorer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updatedgame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameLevel");

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("931e2a1d-7766-4f9f-9ced-f78b57a8faf4"));

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "SpentTime",
                table: "Games");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Games",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Games",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "ImageId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("eaa88c90-9c31-4e69-aa13-7227f83d897d"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValue: new Guid("931e2a1d-7766-4f9f-9ced-f78b57a8faf4"));

            migrationBuilder.CreateTable(
                name: "GameLevels",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "integer", nullable: false),
                    LevelId = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLevels", x => new { x.GameId, x.LevelId });
                    table.ForeignKey(
                        name: "FK_GameLevels_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameLevels_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "Name", "Path" },
                values: new object[] { new Guid("eaa88c90-9c31-4e69-aa13-7227f83d897d"), "", "" });

            migrationBuilder.CreateIndex(
                name: "IX_GameLevels_LevelId",
                table: "GameLevels",
                column: "LevelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameLevels");

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("eaa88c90-9c31-4e69-aa13-7227f83d897d"));

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Games",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SpentTime",
                table: "Games",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AlterColumn<Guid>(
                name: "ImageId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("931e2a1d-7766-4f9f-9ced-f78b57a8faf4"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValue: new Guid("eaa88c90-9c31-4e69-aa13-7227f83d897d"));

            migrationBuilder.CreateTable(
                name: "GameLevel",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "integer", nullable: false),
                    LevelsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLevel", x => new { x.GameId, x.LevelsId });
                    table.ForeignKey(
                        name: "FK_GameLevel_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameLevel_Levels_LevelsId",
                        column: x => x.LevelsId,
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "Name", "Path" },
                values: new object[] { new Guid("931e2a1d-7766-4f9f-9ced-f78b57a8faf4"), "", "" });

            migrationBuilder.CreateIndex(
                name: "IX_GameLevel_LevelsId",
                table: "GameLevel",
                column: "LevelsId");
        }
    }
}
