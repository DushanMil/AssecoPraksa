using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssecoPraksa.Migrations
{
    /// <inheritdoc />
    public partial class AddedCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    code = table.Column<string>(type: "text", nullable: false),
                    parentcode = table.Column<string>(name: "parent-code", type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.code);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
