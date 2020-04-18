 using Microsoft.EntityFrameworkCore.Migrations;

namespace alderam.stocks.api.Migrations
{
    public partial class AddicionandoDadosNaTabelaDeSetores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('COMERCIO', 'Serviços Comerciais');");
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('COMUNICACAO', 'Comunicações');"); 
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('DURAVEIS', 'Consumíveis Duráveis');"); 
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('NAO_DURAVEIS', 'Consumo Não-Duráveis'); ");
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('SERVICO', 'Consumo de Serviços'); ");
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('LOGISTICA', 'Serviços de Logistica');"); 
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('ELETRONICA', 'Tecnologia Eletrônica');"); 
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('MINERACAO', 'Minerais Energéticos');"); 
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('FINANCEIRO', 'Financeiro');"); 
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('GOV', 'Governo');"); 
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('SAUDE', 'Serviços de Saúde');"); 
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('TEC_SAUDE', 'Tecnologia em Saúde');"); 
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('INDUSTRIA', 'Serviços Industriais');"); 
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('MISC', 'Miscelânea');");
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('ENERGIA', 'Minerais não Energéticos');");
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('INDUSTRIA', 'Industrias de Processamento');");
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('ARTESANAL', 'Produção Artesanal');"); 
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('VAREJO', 'Comercio de Varejo');"); 
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('SERV_TEC', 'Serviços de Tecnologia');");
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('TRANSP', 'Transportes');"); 
            migrationBuilder.Sql("insert into Setores (Codigo, Nome) values ('SERV_PUBL', 'Serviços Públicos');"); 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
