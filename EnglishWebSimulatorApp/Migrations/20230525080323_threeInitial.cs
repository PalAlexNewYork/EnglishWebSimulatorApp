using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EnglishWebSimulatorApp.Migrations
{
    public partial class threeInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sound",
                table: "libraryEns");

            migrationBuilder.AddColumn<string>(
                name: "SoundFilePath",
                table: "libraryEns",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoundFilePath",
                table: "libraryEns");

            migrationBuilder.AddColumn<byte[]>(
                name: "Sound",
                table: "libraryEns",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
