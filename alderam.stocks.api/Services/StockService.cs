using alderam.stocks.api.Database;
using alderam.stocks.api.Models;
using alderam.stocks.api.Models.DTOs;
using AutoMapper;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alderam.stocks.api.Services
{
    public interface IStockService
    {
        Task<Boleta> IncluirBoleta(BoletaDTO boletaRequest);
        Task<Boleta> AtualizarBoleta(BoletaDTO boletaRequest);
        double CalcularCorretagem(IEnumerable<Operacao> operacoes);
        double CalcularTaxaDeLiquidacao(IEnumerable<Operacao> operacoes);
        double CalcularEmolumentos(IEnumerable<Operacao> operacoes);
        double CalcularISS(double valorDaCorretagem);
    }

    public class StockService : IStockService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public StockService(IMapper mapper,
                            DatabaseContext databaseContext)
        {
            _mapper = mapper;
            _databaseContext = databaseContext;
        }

        public async Task<Boleta> IncluirBoleta(BoletaDTO boletaRequest)
        {
            foreach (var operacao in boletaRequest.Operacoes)
            {
                operacao.Ativo = await CriarAtivo(operacao.CodigoDoAtivo);
                operacao.DataDaOperacao = operacao.DataDaOperacao;
                operacao.DataDeCriacao = DateTime.Now; 
            }

            var boleta = _mapper.Map<Boleta>(boletaRequest);

            boleta.Emolumentos = CalcularEmolumentos(boleta.Operacoes);
            boleta.Corretagem = CalcularCorretagem(boleta.Operacoes);
            boleta.ISS = CalcularISS(boleta.Corretagem);
            boleta.TaxaDeLiquidacao = CalcularTaxaDeLiquidacao(boleta.Operacoes);

            boleta.DataDaOperacao = boletaRequest.DataDaOperacao;
            boleta.DataDeCriacao = DateTime.Now;

            _databaseContext.Boletas.Add(boleta);
            await _databaseContext.SaveChangesAsync();

            return boleta;
        }

        public async Task<Boleta> AtualizarBoleta(BoletaDTO boletaRequest)
        {
            var boleta = _databaseContext.Boletas.Single(r => r.Id == boletaRequest.Id);

            foreach (var operacao in boletaRequest.Operacoes)
            {
                operacao.Ativo = await CriarAtivo(operacao.CodigoDoAtivo);
                operacao.DataDaOperacao = operacao.DataDaOperacao;
            } 

            boleta = _mapper.Map<Boleta>(boletaRequest);

            boleta.Emolumentos = CalcularEmolumentos(boleta.Operacoes);
            boleta.Corretagem = CalcularCorretagem(boleta.Operacoes);
            boleta.ISS = CalcularISS(boleta.Corretagem);
            boleta.TaxaDeLiquidacao = CalcularTaxaDeLiquidacao(boleta.Operacoes);

            boleta.DataDaOperacao = boletaRequest.DataDaOperacao;
 
            await _databaseContext.SaveChangesAsync();

            return boleta;
        }

        public async Task<Ativo> CriarAtivo(string codigoDoAtivo)
        {
            var ativo = _databaseContext.Ativos.SingleOrDefault(r => r.Codigo == codigoDoAtivo);

            if (ativo == null)
            {
                ativo = new Ativo()
                {
                    Codigo = codigoDoAtivo,
                    Nome = codigoDoAtivo,
                    DataDeCriacao = DateTime.Now
                };

                _databaseContext.Ativos.Add(ativo);
                await _databaseContext.SaveChangesAsync();
            }
                
            return ativo;
        }

        public double CalcularCorretagem(IEnumerable<Operacao> operacoes)
        {
            var valorDaOperacao = operacoes.Sum(o => (o.PrecoDeCompra * o.Quantitidade));
            var result = (10 + (valorDaOperacao * 0.003) ) + 10;
            return result;
        } // = (10+((F5)*0.003))+10

        public double CalcularTaxaDeLiquidacao(IEnumerable<Operacao> operacoes)
        {
            var valorDaOperacao = operacoes.Sum(o => (o.PrecoDeCompra * o.Quantitidade));
            var result = (valorDaOperacao * 0.0275) / 100;
            return result;
        } // =(F5*0.0275)/100

        public double CalcularEmolumentos(IEnumerable<Operacao> operacoes)
        {
            var valorDaOperacao = operacoes.Sum(o => (o.PrecoDeCompra * o.Quantitidade));
            var result = ((valorDaOperacao * 0.003248) / 100);
            return result;
        } // = ((F5*0.003248)/100) 

        public double CalcularISS(double valorDaCorretagem)
        {
            var result = ((valorDaCorretagem / 0.95) - valorDaCorretagem);
            return result;
        } // = ((J5/0.95)-J5) 
    }
}