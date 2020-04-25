using Microsoft.EntityFrameworkCore.Migrations;

namespace alderam.stocks.api.Migrations
{
    public partial class AdicionandoTaxaDeEmolumentos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TaxaDaEmolumentos",
                table: "Boletas",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxaDaEmolumentos",
                table: "Boletas");
        }
    }
}
