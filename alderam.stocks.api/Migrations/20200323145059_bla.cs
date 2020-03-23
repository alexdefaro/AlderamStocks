using Microsoft.EntityFrameworkCore.Migrations;

namespace alderam.stocks.api.Migrations
{
    public partial class bla : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Corretagem",
                table: "Operacoes");

            migrationBuilder.DropColumn(
                name: "Emolumentos",
                table: "Operacoes");

            migrationBuilder.DropColumn(
                name: "ISS",
                table: "Operacoes");

            migrationBuilder.DropColumn(
                name: "TaxaDeLiquidacao",
                table: "Operacoes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Corretagem",
                table: "Operacoes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Emolumentos",
                table: "Operacoes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ISS",
                table: "Operacoes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TaxaDeLiquidacao",
                table: "Operacoes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
