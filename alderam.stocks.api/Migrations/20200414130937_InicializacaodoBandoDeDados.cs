using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace alderam.stocks.api.Migrations
{
    public partial class InicializacaodoBandoDeDados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boletas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(maxLength: 30, nullable: false),
                    DataDaOperacao = table.Column<DateTime>(nullable: false),
                    TaxaDeLiquidacao = table.Column<decimal>(nullable: false),
                    Emolumentos = table.Column<decimal>(nullable: false),
                    Corretagem = table.Column<decimal>(nullable: false),
                    ISS = table.Column<decimal>(nullable: false),
                    DataDeCriacao = table.Column<DateTime>(nullable: false),
                    ValorDaOperacao = table.Column<decimal>(nullable: false),
                    ValorDaCompra = table.Column<decimal>(nullable: false),
                    Observacoes = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boletas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Setores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(maxLength: 20, nullable: false),
                    Nome = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ativos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(maxLength: 10, nullable: false),
                    Nome = table.Column<string>(maxLength: 100, nullable: false),
                    DataDeCriacao = table.Column<DateTime>(nullable: false),
                    SetorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ativos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ativos_Setores_SetorId",
                        column: x => x.SetorId,
                        principalTable: "Setores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Acompanhamentos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AtivoId = table.Column<int>(nullable: false),
                    PrecoAtual = table.Column<double>(nullable: false),
                    PrecoDeCompra = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acompanhamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Acompanhamentos_Ativos_AtivoId",
                        column: x => x.AtivoId,
                        principalTable: "Ativos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operacoes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoletaId = table.Column<int>(nullable: false),
                    AtivoId = table.Column<int>(nullable: false),
                    DataDaOperacao = table.Column<DateTime>(nullable: false),
                    Quantitidade = table.Column<int>(nullable: false),
                    PrecoDeCompra = table.Column<decimal>(nullable: false),
                    DataDeCriacao = table.Column<DateTime>(nullable: false),
                    ValorDaOperacao = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operacoes_Ativos_AtivoId",
                        column: x => x.AtivoId,
                        principalTable: "Ativos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operacoes_Boletas_BoletaId",
                        column: x => x.BoletaId,
                        principalTable: "Boletas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Acompanhamentos_AtivoId",
                table: "Acompanhamentos",
                column: "AtivoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ativos_Codigo",
                table: "Ativos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ativos_SetorId",
                table: "Ativos",
                column: "SetorId");

            migrationBuilder.CreateIndex(
                name: "IX_Operacoes_AtivoId",
                table: "Operacoes",
                column: "AtivoId");

            migrationBuilder.CreateIndex(
                name: "IX_Operacoes_BoletaId",
                table: "Operacoes",
                column: "BoletaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Acompanhamentos");

            migrationBuilder.DropTable(
                name: "Operacoes");

            migrationBuilder.DropTable(
                name: "Ativos");

            migrationBuilder.DropTable(
                name: "Boletas");

            migrationBuilder.DropTable(
                name: "Setores");
        }
    }
}
