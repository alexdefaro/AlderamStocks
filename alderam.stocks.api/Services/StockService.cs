using alderam.stocks.api.Database;
using alderam.stocks.api.Models;
using alderam.stocks.api.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        double CalculateCorretagem(IEnumerable<Operacao> operacoes);
        double CalculateTaxaDeLiquidacao(IEnumerable<Operacao> operacoes);
        double CalculateEmolumentos(IEnumerable<Operacao> operacoes);
        double CalculateISS(double valorDaCorretagem);
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
                var ativo = _databaseContext.Ativos.SingleOrDefault(r => r.Codigo == operacao.CodigoDoAtivo);

                if (ativo == null)
                {
                    ativo = new Ativo()
                    {
                        Codigo = operacao.CodigoDoAtivo,
                        Nome = operacao.CodigoDoAtivo,
                        DataDeCriacao = DateTime.Now
                    };
                }

                operacao.Ativo = ativo;
                operacao.DataDaOperacao = boletaRequest.dataDaOperacao;
                operacao.DataDeCriacao = DateTime.Now;
            }

            var boleta = new Boleta();

            boleta = _mapper.Map<Boleta>(boletaRequest);
            boleta.Emolumentos = this.CalculateEmolumentos(boleta.Operacoes);
            boleta.Corretagem = this.CalculateCorretagem(boleta.Operacoes);
            boleta.ISS = this.CalculateISS(boleta.Corretagem);
            boleta.TaxaDeLiquidacao = this.CalculateTaxaDeLiquidacao(boleta.Operacoes);

            boleta.DataDaOperacao = boletaRequest.dataDaOperacao;
            boleta.DataDeCriacao = DateTime.Now;

            _databaseContext.Boletas.Add(boleta);
            await _databaseContext.SaveChangesAsync();

            return boleta;
        }

        public async Task<Boleta> AtualizarBoleta(BoletaDTO boletaRequest)
        {
            foreach (var operacao in boletaRequest.Operacoes)
            {
                var ativo = _databaseContext.Ativos.SingleOrDefault(r => r.Codigo == operacao.CodigoDoAtivo);

                if (ativo == null)
                {
                    ativo = new Ativo()
                    {
                        Codigo = operacao.CodigoDoAtivo,
                        Nome = operacao.CodigoDoAtivo,
                        DataDeCriacao = DateTime.Now
                    };
                }

                operacao.Ativo = ativo;
            }

            var boleta = new Boleta();

            boleta = _mapper.Map<Boleta>(boletaRequest);
            boleta.Emolumentos = this.CalculateEmolumentos(boleta.Operacoes);
            boleta.Corretagem = this.CalculateCorretagem(boleta.Operacoes);
            boleta.ISS = this.CalculateISS(boleta.Corretagem);
            boleta.TaxaDeLiquidacao = this.CalculateTaxaDeLiquidacao(boleta.Operacoes);

            boleta.DataDaOperacao = boletaRequest.dataDaOperacao;
            boleta.DataDeCriacao = DateTime.Now;

            _databaseContext.Boletas.Add(boleta);
            await _databaseContext.SaveChangesAsync();

            return boleta;
        }

        public double CalculateCorretagem(IEnumerable<Operacao> operacoes)
        {
            var valorDaOperacao = operacoes.Sum(o => (o.PrecoDeCompra * o.Quantitidade));
            var result = (10 + (valorDaOperacao * 0.003) ) + 10;
            return result;
        } // = (10+((F5)*0.003))+10

        public double CalculateTaxaDeLiquidacao(IEnumerable<Operacao> operacoes)
        {
            var valorDaOperacao = operacoes.Sum(o => (o.PrecoDeCompra * o.Quantitidade));
            var result = (valorDaOperacao * 0.0275) / 100;
            return result;
        } // =(F5*0.0275)/100

        public double CalculateEmolumentos(IEnumerable<Operacao> operacoes)
        {
            var valorDaOperacao = operacoes.Sum(o => (o.PrecoDeCompra * o.Quantitidade));
            var result = ((valorDaOperacao * 0.003248) / 100);
            return result;
        } // = ((F5*0.003248)/100) 

        public double CalculateISS(double valorDaCorretagem)
        {
            var result = ((valorDaCorretagem / 0.95) - valorDaCorretagem);
            return result;
        } // = ((J5/0.95)-J5) 
    }
}