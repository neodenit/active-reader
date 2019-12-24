using Microsoft.EntityFrameworkCore.Migrations;

namespace Neodenit.ActiveReader.DataAccess.Migrations
{
    public partial class AddArticleOwnerColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Articles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Articles");
        }
    }
}
