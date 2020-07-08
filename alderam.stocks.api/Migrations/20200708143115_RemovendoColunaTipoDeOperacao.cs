using Microsoft.EntityFrameworkCore.Migrations;

namespace alderam.stocks.api.Migrations
{
    public partial class RemovendoColunaTipoDeOperacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoDeOperacao",
                table: "Operacoes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoDeOperacao",
                table: "Operacoes",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);
        }
    }
}
