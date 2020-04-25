using Microsoft.EntityFrameworkCore.Migrations;

namespace alderam.stocks.api.Migrations
{
    public partial class RefatorandoCalculoDeEmolumentos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxaDaEmolumentos",
                table: "Boletas");

            migrationBuilder.AddColumn<bool>(
                name: "OperacaoEmLeilao",
                table: "Boletas",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperacaoEmLeilao",
                table: "Boletas");

            migrationBuilder.AddColumn<decimal>(
                name: "TaxaDaEmolumentos",
                table: "Boletas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
