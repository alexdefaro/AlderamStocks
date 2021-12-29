using Microsoft.EntityFrameworkCore.Migrations;

namespace alderam.stocks.api.Migrations
{
    public partial class IncluinCampoListarNaTabelaAtivos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Listar",
                table: "Ativos",
                nullable: false,
                defaultValue: "S");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Listar",
                table: "Ativos");
        }
    }
}
