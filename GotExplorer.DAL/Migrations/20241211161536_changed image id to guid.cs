using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GotExplorer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class changedimageidtoguid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Images_ImageId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Images");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Images",
                type: "uuid",
                nullable: false)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.Sequence);

            migrationBuilder.AddPrimaryKey(
                 name: "PK_Images",
                 table: "Images",
                 column: "Id");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("931e2a1d-7766-4f9f-9ced-f78b57a8faf4"));

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Images_ImageId",
                table: "AspNetUsers",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "Name", "Path" },
                values: new object[] { new Guid("931e2a1d-7766-4f9f-9ced-f78b57a8faf4"), "", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("931e2a1d-7766-4f9f-9ced-f78b57a8faf4"));

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Images_ImageId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Images");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Images",
                type: "integer",
                nullable: false)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: new Guid("931e2a1d-7766-4f9f-9ced-f78b57a8faf4"));

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Images_ImageId",
                table: "AspNetUsers",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddPrimaryKey(
                 name: "IPK_Imagesd",
                 table: "Images",
                 column: "Id");

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "Name", "Path" },
                values: new object[] { 1, "", "" });
        }
    }
}
