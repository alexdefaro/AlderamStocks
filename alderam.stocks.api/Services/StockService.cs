using alderam.stocks.api.Database;
using alderam.stocks.api.Models;
using alderam.stocks.api.Models.DTOs;
using AutoMapper;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Newtonsoft.Json;

namespace alderam.stocks.api.Services
{
    public class ResultadoDaAPIHG
    {
        [JsonProperty("results")]
        public Dictionary<string, CotacaoHG> Results { get; set; }
    }

    public class CotacaoHG
    {
        [JsonProperty("price")]
        public decimal Price { get; set; }
    }

    public class ResultadoDaAPI
    {
        [JsonProperty("Global Quote")]
        public Cotacao Cotacao { get; set; }

        public string Note { get; set; }
    }

    public class Cotacao
    {
        [JsonProperty("01. symbol")]
        public string Symbol { get; set; }

        [JsonProperty("02. open")]
        public decimal Open { get; set; }

        [JsonProperty("03. high")]
        public decimal High { get; set; }

        [JsonProperty("04. low")]
        public decimal Low { get; set; }

        [JsonProperty("05. price")]
        public decimal Price { get; set; }

        [JsonProperty("06. volume")]
        public decimal Volume { get; set; }

        [JsonProperty("07. latest trading day")]
        public DateTime LastTradingDay { get; set; }

        [JsonProperty("08. previous close")]
        public decimal PreviousClose { get; set; }

        [JsonProperty("09. change")]
        public decimal Change { get; set; }

        [JsonProperty("10. change percent")]
        public string ChangePercent { get; set; }
    }

    public interface IStockService
    {
        Task CarregarCotacoes();

        Task<ResumoDTO> RecuperarResumoDaCarteira();
        Task<GraficoDeSetoresDTO> RecuperarDadosDoGraficoDeSetores(TiposDeInvestimento tipoDeInvestimento);

        // Operacoes
        Task<IEnumerable<Operacao>> RecuperarOperacoes();


        // Ativos 
        Task<IEnumerable<Ativo>> RecuperarAtivos(int? id = null);
        Task<Ativo> RecuperarAtivo(int id);

        // Setores
        Task<IEnumerable<Setor>> RecuperarSetores(int? id = null);
        Task<Setor> RecuperarSetor(int id);

        // Acompanhamento
        Task<IEnumerable<Acompanhamento>> RecuperarAcompanhamentos(int? id = null);
        Task<Acompanhamento> RecuperarAcompanhamento(int id);
        Task<bool> ExcluirAcompanhamento(int id);
        Task<Acompanhamento> IncluirAcompanhamento(AcompanhamentoDTO requestDTO);
        Task<Acompanhamento> AtualizarAcompanhamento(AcompanhamentoDTO requestDTO);

        // Boletas
        Task<IEnumerable<Boleta>> RecuperarBoletas(int? id = null);
        Task<Boleta> RecuperarBoleta(int id);
        Task<bool> ExcluirBoleta(int id);
        Task<Boleta> IncluirBoleta(BoletaDTO boletaRequest);
        Task<Boleta> AtualizarBoleta(BoletaDTO boletaRequest);

        decimal CalcularCorretagem(decimal taxaDaCoretagem, IEnumerable<Operacao> operacoes);
        decimal CalcularTaxaDeLiquidacao(IEnumerable<Operacao> operacoes);
        decimal CalcularEmolumentos(bool operacaoEmLeilao, IEnumerable<Operacao> operacoes);
        decimal CalcularISS(decimal valorDaCorretagem);
    }

    public class StockService : IStockService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        private async Task CarregarCotacoesHG()
        {
            var ativos = await _databaseContext.Ativos.ToListAsync();
            var client = new HttpClient();

            foreach (var ativo in ativos)
            {
                if (!ativo.DataDaUltimaCotacao.HasValue || ativo.DataDaUltimaCotacao < DateTime.Now.AddMinutes(-2))
                {
                    var url = $"https://api.hgbrasil.com/finance/stock_price?key=58a1af63&symbol={ativo.Codigo}";
                    var response = await client.GetAsync(url);
                    var content = await response.Content.ReadAsStringAsync();
                    var resultadoDaAPI = JsonConvert.DeserializeObject<ResultadoDaAPIHG>(content);

                    if (resultadoDaAPI.Results[ativo.Codigo].Price == 0)
                    {
                        await CarregarCotacoesAV(ativo.Codigo);
                        continue;
                    }

                    if (resultadoDaAPI.Results != null)
                    {
                        ativo.PrecoAnterior = ativo.PrecoAtual;
                        ativo.PrecoAtual = resultadoDaAPI.Results[ativo.Codigo].Price;
                        ativo.DataDaUltimaCotacao = DateTime.Now;
                    }
                }

            }

            await _databaseContext.SaveChangesAsync();
        }

        private async Task CarregarCotacoesAV(string codigoDoAtivo = null)
        {
            var ativos = await _databaseContext.Ativos.ToListAsync();
            var client = new HttpClient();

            int contador = 1;
            foreach (var ativo in ativos)
            {
                if (codigoDoAtivo != null && codigoDoAtivo != ativo.Codigo)
                {
                    continue;
                }

                if (!ativo.DataDaUltimaCotacao.HasValue || ativo.DataDaUltimaCotacao < DateTime.Now.AddMinutes(-5))
                {
                    var url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={ativo.Codigo}.SA&apikey=0GQ6IW3IL3BFJGTJl";
                    var response = await client.GetAsync(url);
                    var resultadoDaAPI = JsonConvert.DeserializeObject<ResultadoDaAPI>(await response.Content.ReadAsStringAsync());

                    if (resultadoDaAPI.Cotacao != null)
                    {
                        ativo.PrecoAnterior = ativo.PrecoAtual;
                        ativo.PrecoAtual = resultadoDaAPI.Cotacao.Price;
                        ativo.DataDaUltimaCotacao = DateTime.Now;
                    }

                    contador++;
                }

                if (contador == 6)
                {
                    await Task.Delay(60000);
                    contador = 0;
                }
            }

            await _databaseContext.SaveChangesAsync();
        }

        public StockService(IMapper mapper,
                            DatabaseContext databaseContext)
        {
            _mapper = mapper;
            _databaseContext = databaseContext;
        }

        private decimal Truncate(decimal value)
        {
            var result = Math.Truncate(100 * value) / 100;
            return result;
        }

        public async Task CarregarCotacoes()
        {
            if (DateTime.Now.Hour < 22)
                await CarregarCotacoesHG();
            else
                await CarregarCotacoesAV();
        }

        public async Task<ResumoDTO> RecuperarResumoDaCarteira()
        {
            var resumo = new ResumoDTO();

            resumo.DataDaUltimaAtualizacao = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

            resumo.ValorAtualDaCarteiral = await _databaseContext.Operacoes.SumAsync(o => o.Quantitidade * o.Ativo.PrecoAtual.Value);
            resumo.ValorTotalInvestido = await _databaseContext.Boletas.SumAsync(s => s.ValorDaOperacao);

            resumo.SaldoAtualDaCarteiral = (resumo.ValorAtualDaCarteiral - resumo.ValorTotalInvestido);

            resumo.LiquidezAtualDaCarteiral = await _databaseContext.Operacoes
                .Include(i => i.Ativo)
                .GroupBy(g => new { Id = g.Ativo.Id, PrecoAtual = g.Ativo.PrecoAtual.Value })
                .Select(o => new { Valor = o.Sum(s => (s.Quantitidade * o.Key.PrecoAtual) - s.ValorDaOperacao) })
                .Select(r => r.Valor)
                .Where(r => r > 0)
                .SumAsync();

            resumo.SaldoAtualDaCarteiralEmPercentual = ((resumo.SaldoAtualDaCarteiral / resumo.ValorTotalInvestido) * 100);
            resumo.LiquidezAtualDaCarteiralEmPercentual = ((resumo.LiquidezAtualDaCarteiral / resumo.ValorTotalInvestido) * 100);

            return resumo;
        }

        public async Task<GraficoDeSetoresDTO> RecuperarDadosDoGraficoDeSetores(TiposDeInvestimento tipoDeInvestimento = TiposDeInvestimento.Acao)
        {
            //var registros = await _databaseContext.Operacoes
            //    .Include(i => i.Ativo.Subsetor)
            //    .Where(r => r.Ativo.TipoDeInvestimento == tipoDeInvestimento)
            //    .GroupBy(g => new { g.Ativo.Subsetor.Nome, PrecoAtual = _databaseContext.Ativos.Single(f => f.Id == g.Id).PrecoAtual.Value })
            //    .Select(g => new { Labels = g.Key.Nome, Values = (g.Sum(r => r.Quantitidade * g.Key.PrecoAtual)) })
            //    .OrderBy(o => o.Labels)
            //    .ToListAsync();

            var registros = await _databaseContext.Operacoes
                .Include(i => i.Ativo.Subsetor)
                .Where(r => r.Ativo.TipoDeInvestimento == tipoDeInvestimento)
                .Select(r => new { NomeDoSubsetor = r.Ativo.Subsetor.Nome, Quantitidade = (r.Quantitidade), Valor = (r.Quantitidade * r.Ativo.PrecoAtual.Value) })
                .GroupBy(g => new { g.NomeDoSubsetor })
                .Select(g => new { Labels = g.Key.NomeDoSubsetor, Quantitidades = g.Sum(r => r.Quantitidade), Values = g.Sum(r => r.Valor) })
                .Where(r => r.Quantitidades > 0)
                .OrderBy(o => o.Labels)
                .ToListAsync();

            var result = new GraficoDeSetoresDTO()
            {
                Labels = registros.Select(r => r.Labels).ToArray(),
                Values = registros.Select(r => r.Values).ToArray()
            };

            return result;
        }

        // Operacoes
        public async Task<IEnumerable<Operacao>> RecuperarOperacoes()
        {
            var registros = await _databaseContext.Operacoes
                .Include(i => i.Boleta)
                .Include(i => i.Ativo.Subsetor)
                .OrderBy(o => o.DataDaOperacao)
                .ToListAsync();

            return registros;
        }

        // Ativos 
        public async Task<IEnumerable<Ativo>> RecuperarAtivos(int? id = null)
        {
            var registros = _databaseContext.Ativos
                    .OrderBy(o => o.Nome);

            if (id != null)
            {
                await registros.SingleAsync(r => r.Id == id);
            }

            return registros;
        }

        public async Task<Ativo> RecuperarAtivo(int id)
        {
            var registro = await RecuperarAtivos(id);

            return registro.First();
        }


        // Setores
        public async Task<IEnumerable<Setor>> RecuperarSetores(int? id = null)
        {
            var registros = _databaseContext.Setores
                    .OrderBy(o => o.Nome);

            if (id != null)
            {
                await registros.SingleAsync(r => r.Id == id);
            }

            return registros;
        }

        public async Task<Setor> RecuperarSetor(int id)
        {
            var registro = await RecuperarSetores(id);

            return registro.First();
        }


        // Acompanhamentos
        public async Task<IEnumerable<Acompanhamento>> RecuperarAcompanhamentos(int? id = null)
        {
            var registros = _databaseContext.Acompanhamentos
                .Include(i => i.Ativo)
                .OrderBy(o => o.Ativo.Nome);

            if (id != null)
            {
                await registros.SingleAsync(r => r.Id == id);
            }

            return registros;
        }

        public async Task<Acompanhamento> RecuperarAcompanhamento(int id)
        {
            var registro = await RecuperarAcompanhamentos(id);

            return registro.First();
        }

        public async Task<Acompanhamento> IncluirAcompanhamento(AcompanhamentoDTO requestDTO)
        {
            requestDTO.Ativo = await SalvarAtivo(requestDTO.CodigoDoAtivo, requestDTO.nomeDoAtivo, 1);

            var registro = _mapper.Map<Acompanhamento>(requestDTO);

            _databaseContext.Acompanhamentos.Add(registro);
            await _databaseContext.SaveChangesAsync();

            return registro;
        }

        public async Task<bool> ExcluirAcompanhamento(int id)
        {
            var registro = await _databaseContext.Acompanhamentos
                .Include(i => i.Ativo)
                .SingleAsync(r => r.Id == id);

            var ativo = registro.Ativo;
            var ativoAindaEmUso = await _databaseContext.Operacoes.AnyAsync(r => r.Ativo.Id == ativo.Id);

            _databaseContext.Acompanhamentos.Remove(registro);

            if (ativoAindaEmUso == false)
            {
                _databaseContext.Ativos.Remove(ativo);
            }

            await _databaseContext.SaveChangesAsync();

            return true;
        }

        public async Task<Acompanhamento> AtualizarAcompanhamento(AcompanhamentoDTO requestDTO)
        {
            var registro = await _databaseContext.Acompanhamentos
                .Include(i => i.Ativo)
                .SingleAsync(r => r.Id == requestDTO.Id);

            requestDTO.Ativo = await SalvarAtivo(requestDTO.CodigoDoAtivo, requestDTO.nomeDoAtivo);

            _mapper.Map(requestDTO, registro);

            await _databaseContext.SaveChangesAsync();

            return registro;
        }


        // Boletas
        public async Task<IEnumerable<Boleta>> RecuperarBoletas(int? id = null)
        {
            var boletas = _databaseContext.Boletas
                .Include(i => i.Operacoes)
                    .ThenInclude(i => i.Ativo)
                .OrderByDescending(o => o.DataDaOperacao)
                    .ThenByDescending(o => o.Numero)
                        .ThenByDescending(o => o.Id);

            if (id != null)
            {
                await boletas.SingleAsync(r => r.Id == id);
            }

            return boletas;
        }

        public async Task<Boleta> RecuperarBoleta(int id)
        {
            var boleta = await RecuperarBoletas(id);

            return boleta.First();
        }

        public async Task<Boleta> IncluirBoleta(BoletaDTO boletaRequest)
        {
            decimal valorTotalDaOperacao = 0;
            foreach (var operacao in boletaRequest.Operacoes)
            {
                operacao.Ativo = await SalvarAtivo(operacao.CodigoDoAtivo, operacao.nomeDoAtivo, operacao.tipoDeInvestimento);
                operacao.DataDeCriacao = DateTime.Now;
                operacao.DataDaOperacao = operacao.DataDaOperacao;

                operacao.Quantitidade = (TiposDeOperacao)Char.Parse(operacao.TipoDeOperacao) == TiposDeOperacao.Compra ?
                    operacao.Quantitidade :
                    -operacao.Quantitidade;

                operacao.ValorDaOperacao = (operacao.Quantitidade * operacao.PrecoUnitario);

                valorTotalDaOperacao += operacao.ValorDaOperacao;
            }

            var boleta = _mapper.Map<Boleta>(boletaRequest);

            boleta.Emolumentos = CalcularEmolumentos(boleta.OperacaoEmLeilao, boleta.Operacoes);
            boleta.Corretagem = CalcularCorretagem(boleta.TaxaDaCoretagem, boleta.Operacoes);
            boleta.ISS = CalcularISS(boleta.Corretagem);
            boleta.TaxaDeLiquidacao = CalcularTaxaDeLiquidacao(boleta.Operacoes);

            boleta.ValorDaCompra = Math.Abs(valorTotalDaOperacao);
            boleta.ValorDaOperacao = Math.Abs(Math.Abs(valorTotalDaOperacao) + boleta.Emolumentos + boleta.Corretagem + boleta.ISS + boleta.TaxaDeLiquidacao);

            boleta.DataDaOperacao = boletaRequest.DataDaOperacao;
            boleta.DataDeCriacao = DateTime.Now;

            _databaseContext.Boletas.Add(boleta);
            await _databaseContext.SaveChangesAsync();

            return boleta;
        }

        public async Task<bool> ExcluirBoleta(int id)
        {
            var boleta = await _databaseContext.Boletas
                .Include(i => i.Operacoes)
                .SingleAsync(r => r.Id == id);

            _databaseContext.Boletas.Remove(boleta);
            await _databaseContext.SaveChangesAsync();

            return true;
        }

        public async Task<Boleta> AtualizarBoleta(BoletaDTO boletaRequest)
        {
            var boleta = await _databaseContext.Boletas
                .Include(i => i.Operacoes)
                    .ThenInclude(i => i.Ativo)
                .SingleAsync(r => r.Id == boletaRequest.Id);

            foreach (var operacao in boletaRequest.Operacoes)
            {
                operacao.Ativo = await SalvarAtivo(operacao.CodigoDoAtivo, operacao.nomeDoAtivo, operacao.tipoDeInvestimento);
                operacao.DataDaOperacao = operacao.DataDaOperacao;
            }

            _mapper.Map(boletaRequest, boleta);

            boleta.Emolumentos = CalcularEmolumentos(boleta.OperacaoEmLeilao, boleta.Operacoes);
            boleta.Corretagem = CalcularCorretagem(boleta.TaxaDaCoretagem, boleta.Operacoes);
            boleta.ISS = CalcularISS(boleta.Corretagem);
            boleta.TaxaDeLiquidacao = CalcularTaxaDeLiquidacao(boleta.Operacoes);

            boleta.DataDaOperacao = boletaRequest.DataDaOperacao;

            await _databaseContext.SaveChangesAsync();

            return boleta;
        }

        public async Task<Ativo> SalvarAtivo(string codigo, string nome, int tipoDeInvestimento = 1)
        {
            var ativo = _databaseContext.Ativos.SingleOrDefault(r => r.Codigo == codigo);

            if (ativo == null)
            {
                ativo = new Ativo()
                {
                    Codigo = codigo,
                    Nome = nome,
                    DataDeCriacao = DateTime.Now,
                    TipoDeInvestimento = (TiposDeInvestimento)tipoDeInvestimento
                };

                _databaseContext.Ativos.Add(ativo);
                await _databaseContext.SaveChangesAsync();
            }
            else
            {
                ativo.Nome = nome;
                ativo.TipoDeInvestimento = (TiposDeInvestimento)tipoDeInvestimento;
            }

            return ativo;
        }

        public decimal CalcularCorretagem(decimal taxaDaCoretagem, IEnumerable<Operacao> operacoes)
        {
            var valorDaOperacao = operacoes.Where(r => r.Ativo.TipoDeInvestimento == TiposDeInvestimento.Acao).Sum(o => (o.PrecoUnitario * o.Quantitidade));

            if (valorDaOperacao == 0)
            {
                return 0;
            }

            var result = (10 + (Math.Abs(valorDaOperacao) * 0.003m)) + taxaDaCoretagem;

            if (valorDaOperacao < 0)
                result = -result;

            return Truncate((decimal)result);
        } // = (10+((F5)*0.003))+F6

        public decimal CalcularTaxaDeLiquidacao(IEnumerable<Operacao> operacoes)
        {
            var valorDaOperacaoBruto = operacoes.Sum(o => (o.PrecoUnitario * Math.Abs(o.Quantitidade)));
            var valorDaOperacaoLiquido = operacoes.Sum(o => (o.PrecoUnitario * o.Quantitidade));
            
            var result = (valorDaOperacaoBruto * 0.0275m) / 100;

            if (valorDaOperacaoLiquido < 0)
                result = -result;

            return Truncate((decimal)result);
        } // =(F5*0.0275)/100

        public decimal CalcularEmolumentos(bool operacaoEmLeilao, IEnumerable<Operacao> operacoes)
        {
            var valorDaOperacaoBruto = operacoes.Sum(o => (o.PrecoUnitario * Math.Abs(o.Quantitidade)));
            var valorDaOperacaoLiquido = operacoes.Sum(o => (o.PrecoUnitario * o.Quantitidade));

            decimal taxaDeEmolumentos = (operacaoEmLeilao == true) ? 0.0070m : 0.003248m;

            var result = ((valorDaOperacaoBruto * taxaDeEmolumentos) / 100);

            if (valorDaOperacaoLiquido < 0)
                result = -result;

            return Truncate((decimal)result);
        } // = ((F5*0.003248)/100) 

        public decimal CalcularISS(decimal valorDaCorretagem)
        {
            var result = ((valorDaCorretagem / (decimal)0.95) - valorDaCorretagem);
            return Truncate(result);
        } // = ((J5/0.95)-J5) 
    }
}