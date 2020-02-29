using Microsoft.EntityFrameworkCore.Migrations;

namespace Neodenit.ActiveReader.DataAccess.Migrations
{
    public partial class AddArticleState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Articles",
                nullable: false,
                defaultValue: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Articles");
        }
    }
}
