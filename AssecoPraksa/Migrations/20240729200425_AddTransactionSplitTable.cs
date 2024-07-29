using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AssecoPraksa.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionSplitTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "transaction_splits",
                columns: table => new
                {
                    splitid = table.Column<int>(name: "split-id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    transactionid = table.Column<int>(name: "transaction-id", type: "integer", nullable: false),
                    Catcode = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_splits", x => x.splitid);
                    table.ForeignKey(
                        name: "FK_transaction_splits_categories_Catcode",
                        column: x => x.Catcode,
                        principalTable: "categories",
                        principalColumn: "code");
                    table.ForeignKey(
                        name: "FK_transaction_splits_transactions_transaction-id",
                        column: x => x.transactionid,
                        principalTable: "transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_transaction_splits_Catcode",
                table: "transaction_splits",
                column: "Catcode");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_splits_transaction-id",
                table: "transaction_splits",
                column: "transaction-id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transaction_splits");
        }
    }
}
