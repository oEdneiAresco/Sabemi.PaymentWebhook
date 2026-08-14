using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sabemi.PaymentWebhook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexPagamentoEvento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdTransacao",
                table: "PagamentoEventos",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_PagamentoEventos_IdTransacao",
                table: "PagamentoEventos",
                column: "IdTransacao",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PagamentoEventos_IdTransacao",
                table: "PagamentoEventos");

            migrationBuilder.AlterColumn<string>(
                name: "IdTransacao",
                table: "PagamentoEventos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
