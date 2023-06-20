using Microsoft.EntityFrameworkCore.Migrations;

namespace EnglishWebSimulatorApp.Migrations
{
    public partial class sevenInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdWords",
                table: "Rezults",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdWords",
                table: "Rezults");
        }
    }
}
