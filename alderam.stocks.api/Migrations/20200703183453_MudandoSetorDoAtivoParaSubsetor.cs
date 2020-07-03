using Microsoft.EntityFrameworkCore.Migrations;

namespace alderam.stocks.api.Migrations
{
    public partial class MudandoSetorDoAtivoParaSubsetor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "SubsetorId",
                table: "Ativos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ativos_SubsetorId",
                table: "Ativos",
                column: "SubsetorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ativos_Subsetores_SubsetorId",
                table: "Ativos",
                column: "SubsetorId",
                principalTable: "Subsetores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ativos_Subsetores_SubsetorId",
                table: "Ativos");

            migrationBuilder.DropIndex(
                name: "IX_Ativos_SubsetorId",
                table: "Ativos");

            migrationBuilder.DropColumn(
                name: "SubsetorId",
                table: "Ativos");

            migrationBuilder.AddColumn<int>(
                name: "SetorId",
                table: "Ativos",
                type: "int",
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
    }
}
