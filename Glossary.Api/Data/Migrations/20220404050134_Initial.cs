using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Glossary.Api.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GlossaryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Term = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Definition = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlossaryItems", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GlossaryItems_Term",
                table: "GlossaryItems",
                column: "Term",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GlossaryItems");
        }
    }
}
