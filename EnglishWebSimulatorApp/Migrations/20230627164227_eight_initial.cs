using Microsoft.EntityFrameworkCore.Migrations;

namespace EnglishWebSimulatorApp.Migrations
{
    public partial class eight_initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Thema",
                table: "libraryEns",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thema",
                table: "libraryEns");
        }
    }
}
