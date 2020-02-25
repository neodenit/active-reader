using Microsoft.EntityFrameworkCore.Migrations;

namespace Neodenit.ActiveReader.DataAccess.Migrations
{
    public partial class AddIgnoreCaseColumnToArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IgnoreCase",
                table: "Articles",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IgnoreCase",
                table: "Articles");
        }
    }
}
