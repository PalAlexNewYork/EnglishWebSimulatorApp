using Microsoft.EntityFrameworkCore.Migrations;

namespace EnglishWebSimulatorApp.Migrations
{
    public partial class initialTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User",
                table: "libraryEns");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "libraryEns",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "libraryEns");

            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "libraryEns",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
