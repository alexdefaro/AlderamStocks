using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using alderam.stocks.api.Database;
using alderam.stocks.api.Models;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using alderam.stocks.api.Services;
using alderam.stocks.api.Models.DTOs;
using System.Diagnostics;

namespace alderam.stocks.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarteiraController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public CarteiraController(IMapper mapper,
                                 IStockService stockService,
                                 DatabaseContext databaseContext)
        {
            _mapper = mapper;
            _stockService = stockService;
            _databaseContext = databaseContext;
        }

        [HttpGet]
        [ResponseCache(Duration = 240)]
        public async Task<ActionResult> Get()
        {
            var operacoes = await _stockService.RecuperarOperacoes();

            var result = operacoes
            .GroupBy(g => (g.Ativo.Codigo, g.Ativo.Nome, g.Ativo.PrecoAtual, g.Ativo.PrecoAnterior))
            .Select(r => new
            {
                CodigoDoAtivo = r.Key.Codigo,
                NomeDoAtivo = r.Key.Nome,
                r.Key.PrecoAtual,
                r.Key.PrecoAnterior,
                Quantitidade = r.Sum(s => s.Quantitidade),
                PrecoMedioCompra = (r.Sum(s => s.ValorDaOperacao)/r.Sum(s => s.Quantitidade)),
                PrecoDeCompra = r.Sum(s => s.PrecoDeCompra),
                ValorDaOperacao = r.Sum(s => s.ValorDaOperacao),
                ValorAtual = r.Sum(s => s.Quantitidade) * r.Key.PrecoAtual,
                Rentabilidade = r.Sum(s => ((s.Quantitidade * s.Ativo.PrecoAtual) - s.ValorDaOperacao)) 
            })
            .OrderBy(o => o.CodigoDoAtivo);

            return Ok(result);
        }
    }
}