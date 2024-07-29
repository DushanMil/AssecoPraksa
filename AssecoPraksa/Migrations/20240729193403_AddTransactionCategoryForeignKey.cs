using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssecoPraksa.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionCategoryForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_transactions_Catcode",
                table: "transactions",
                column: "Catcode");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_categories_Catcode",
                table: "transactions",
                column: "Catcode",
                principalTable: "categories",
                principalColumn: "code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_categories_Catcode",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_Catcode",
                table: "transactions");
        }
    }
}
