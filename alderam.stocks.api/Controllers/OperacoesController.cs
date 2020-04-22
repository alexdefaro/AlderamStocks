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
    public class OperacoesController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public OperacoesController(IMapper mapper,
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

            var result = operacoes.Select(r => new
            {
                r.Id,
                r.DataDaOperacao,
                codigoDoAtivo = r.Ativo.Codigo,
                nomeDoAtivo = r.Ativo.Nome,
                r.Quantitidade,
                r.PrecoDeCompra,
                r.ValorDaOperacao,

                PrecoAtual = r.Ativo.PrecoAtual,
                ValorAtual = (r.Quantitidade * r.Ativo.PrecoAtual),
                Rentabilidade = ((r.Quantitidade * r.Ativo.PrecoAtual) - r.ValorDaOperacao)
            })
                .OrderBy(o => o.DataDaOperacao);

            return Ok(result);
        }
    }
}