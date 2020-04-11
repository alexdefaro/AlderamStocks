using Microsoft.EntityFrameworkCore.Migrations;

namespace alderam.stocks.api.Migrations
{
    public partial class AdicionandoSetorNaTabelaDeAtivo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "Setores",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<int>(
                name: "SetorId",
                table: "Ativos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ativos_SetorId",
                table: "Ativos",
                column: "SetorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ativos_Setores_SetorId",
                table: "Ativos",
                column: "SetorId",
                principalTable: "Setores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ativos_Setores_SetorId",
                table: "Ativos");

            migrationBuilder.DropIndex(
                name: "IX_Ativos_SetorId",
                table: "Ativos");

            migrationBuilder.DropColumn(
                name: "SetorId",
                table: "Ativos");

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "Setores",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20);
        }
    }
}
