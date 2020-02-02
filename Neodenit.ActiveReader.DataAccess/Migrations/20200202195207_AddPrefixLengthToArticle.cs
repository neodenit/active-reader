using Microsoft.EntityFrameworkCore.Migrations;

namespace Neodenit.ActiveReader.DataAccess.Migrations
{
    public partial class AddPrefixLengthToArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrefixLength",
                table: "Articles",
                nullable: false,
                defaultValue: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrefixLength",
                table: "Articles");
        }
    }
}
