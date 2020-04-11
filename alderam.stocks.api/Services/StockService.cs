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

namespace alderam.stocks.api.Services
{
    public interface IStockService
    {
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

        public async Task<IEnumerable<Operacao>> RecuperarOperacoes()
        {
            var registros = await _databaseContext.Operacoes
                .Include(i => i.Boleta)
                .Include(i => i.Ativo)
                .OrderBy(o => o.DataDaOperacao)
                .ToListAsync();

            return registros;
        }


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
            requestDTO.Ativo = await CriarAtivo(requestDTO.CodigoDoAtivo);

            var registro = _mapper.Map<Acompanhamento>(requestDTO);
            
            _databaseContext.Acompanhamentos.Add(registro);
            await _databaseContext.SaveChangesAsync();

            return registro;
        }

        public async Task<bool> ExcluirAcompanhamento(int id)
        {
            var registro = await _databaseContext.Acompanhamentos
                .SingleAsync(r => r.Id == id);

            _databaseContext.Acompanhamentos.Remove(registro);
            await _databaseContext.SaveChangesAsync();

            return true;
        }

        public async Task<Acompanhamento> AtualizarAcompanhamento(AcompanhamentoDTO requestDTO)
        {
            var registro = await _databaseContext.Acompanhamentos
                .Include(i => i.Ativo)
                .SingleAsync(r => r.Id == requestDTO.Id);

            requestDTO.Ativo = await CriarAtivo(requestDTO.CodigoDoAtivo);

            _mapper.Map(requestDTO, registro);

            await _databaseContext.SaveChangesAsync();

            return registro;
        }


        public async Task<IEnumerable<Boleta>> RecuperarBoletas(int? id = null)
        {
            var boletas = _databaseContext.Boletas
                .Include(i => i.Operacoes)
                    .ThenInclude(i => i.Ativo);

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
                operacao.Ativo = await CriarAtivo(operacao.CodigoDoAtivo);
                operacao.DataDaOperacao = operacao.DataDaOperacao;
            }

            _mapper.Map(boletaRequest, boleta);

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
            var result = (10 + (valorDaOperacao * 0.003)) + 10;
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