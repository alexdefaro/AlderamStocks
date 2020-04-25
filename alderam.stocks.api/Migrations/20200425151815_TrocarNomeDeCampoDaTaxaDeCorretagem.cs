using Microsoft.EntityFrameworkCore.Migrations;

namespace alderam.stocks.api.Migrations
{
    public partial class TrocarNomeDeCampoDaTaxaDeCorretagem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorDaCoretagem",
                table: "Boletas");

            migrationBuilder.AddColumn<decimal>(
                name: "TaxaDaCoretagem",
                table: "Boletas",
                nullable: false,
                defaultValue: 10m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxaDaCoretagem",
                table: "Boletas");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorDaCoretagem",
                table: "Boletas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
