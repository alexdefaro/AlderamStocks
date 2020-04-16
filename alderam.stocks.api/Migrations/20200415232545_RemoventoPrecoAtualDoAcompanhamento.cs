using Microsoft.EntityFrameworkCore.Migrations;

namespace alderam.stocks.api.Migrations
{
    public partial class RemoventoPrecoAtualDoAcompanhamento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrecoAtual",
                table: "Acompanhamentos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PrecoAtual",
                table: "Acompanhamentos",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
