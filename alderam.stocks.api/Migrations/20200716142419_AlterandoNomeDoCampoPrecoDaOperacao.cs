using Microsoft.EntityFrameworkCore.Migrations;

namespace alderam.stocks.api.Migrations
{
    public partial class AlterandoNomeDoCampoPrecoDaOperacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrecoDeCompra",
                table: "Operacoes");

            migrationBuilder.AddColumn<decimal>(
                name: "PrecoUnitario",
                table: "Operacoes",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrecoUnitario",
                table: "Operacoes");

            migrationBuilder.AddColumn<decimal>(
                name: "PrecoDeCompra",
                table: "Operacoes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
