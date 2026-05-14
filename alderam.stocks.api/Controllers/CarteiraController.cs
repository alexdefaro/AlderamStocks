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
using Microsoft.AspNetCore.Authorization;

namespace alderam.stocks.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        public async Task<ActionResult> Get(DateTime? dataLimite)
        { 
            var operacoes = await _databaseContext.Operacoes
                .Include(i => i.Ativo.Subsetor)
                .Where(r => r.Ativo.Listar == 'S')
                .OrderBy(o => o.DataDaOperacao)
                .ToListAsync();

            var result = operacoes
                .Where(r => r.DataDaOperacao <= dataLimite)
                .GroupBy(g => new { g.Ativo.Id, g.Ativo.Codigo, g.Ativo.Nome, g.Ativo.TipoDeInvestimento, NomeDoSetor = g.Ativo.Subsetor.Nome, g.Ativo.PrecoAtual, g.Ativo.PrecoAnterior })
                .Where(r => r.Sum(s => s.Quantitidade) > 0)
                .Select(r => new
                {
                    CodigoDoAtivo = r.Key.Codigo,
                    NomeDoAtivo = r.Key.Nome,
                    r.Key.TipoDeInvestimento,
                    r.Key.NomeDoSetor,
                    r.Key.PrecoAtual,
                    r.Key.PrecoAnterior,
                    Quantitidade = r.Sum(s => s.Quantitidade),
                    PrecoMedioCompra = (r.Sum(s => s.Quantitidade) == 0 ? 0 : (r.Sum(s => s.ValorDaOperacao)/r.Sum(s => s.Quantitidade))),
                    PrecoUnitario = r.Sum(s => s.PrecoUnitario),
                    ValorDaOperacao = r.Sum(s => s.ValorDaOperacao),
                    ValorAtual = r.Sum(s => s.Quantitidade) * r.Key.PrecoAtual,
                    Rentabilidade = r.Sum(s => ((s.Quantitidade * s.Ativo.PrecoAtual) - s.ValorDaOperacao)),
                    Comprar = r.Key.PrecoAtual.HasValue && 
                        (r.Key.PrecoAtual.Value < (_databaseContext.Acompanhamentos.Include(i => i.Ativo).SingleOrDefault(f => f.Ativo.Id == r.Key.Id)?.PrecoDeCompra ?? r.Key.PrecoAtual.Value)) 
                })
                .OrderBy(o => o.TipoDeInvestimento)
                    .ThenBy(o => o.CodigoDoAtivo);

            Response.Headers.Append("X-Total-Count", result.Count().ToString());

            return Ok(result);
        }
    }
}