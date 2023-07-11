using Microsoft.EntityFrameworkCore.Migrations;

namespace EnglishWebSimulatorApp.Migrations
{
    public partial class nine_initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsIdWordsDatabase",
                table: "Rezults",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIdWordsDatabase",
                table: "Rezults");
        }
    }
}
