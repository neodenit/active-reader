using Microsoft.EntityFrameworkCore.Migrations;

namespace Neodenit.ActiveReader.DataAccess.Migrations
{
    public partial class RenameIdColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_Articles_ArticleID",
                table: "Statistics");

            migrationBuilder.DropForeignKey(
                name: "FK_Words_Articles_ArticleID",
                table: "Words");

            migrationBuilder.RenameColumn(
                name: "ArticleID",
                table: "Words",
                newName: "ArticleId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Words",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Words_ArticleID",
                table: "Words",
                newName: "IX_Words_ArticleId");

            migrationBuilder.RenameColumn(
                name: "ArticleID",
                table: "Statistics",
                newName: "ArticleId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Statistics",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Statistics_ArticleID",
                table: "Statistics",
                newName: "IX_Statistics_ArticleId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Articles",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_Articles_ArticleId",
                table: "Statistics",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Words_Articles_ArticleId",
                table: "Words",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_Articles_ArticleId",
                table: "Statistics");

            migrationBuilder.DropForeignKey(
                name: "FK_Words_Articles_ArticleId",
                table: "Words");

            migrationBuilder.RenameColumn(
                name: "ArticleId",
                table: "Words",
                newName: "ArticleID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Words",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Words_ArticleId",
                table: "Words",
                newName: "IX_Words_ArticleID");

            migrationBuilder.RenameColumn(
                name: "ArticleId",
                table: "Statistics",
                newName: "ArticleID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Statistics",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Statistics_ArticleId",
                table: "Statistics",
                newName: "IX_Statistics_ArticleID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Articles",
                newName: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_Articles_ArticleID",
                table: "Statistics",
                column: "ArticleID",
                principalTable: "Articles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Words_Articles_ArticleID",
                table: "Words",
                column: "ArticleID",
                principalTable: "Articles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
