using Microsoft.EntityFrameworkCore.Migrations;

namespace alderam.stocks.api.Migrations
{
    public partial class AdicionandoCampoTipoDeInvestimento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoDeInvestimento",
                table: "Ativos",
                nullable: true);

            migrationBuilder.Sql($"update Ativos set TipoDeInvestimento = '1'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoDeInvestimento",
                table: "Ativos");
        }
    }
}
