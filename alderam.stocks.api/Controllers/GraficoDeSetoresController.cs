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
    public class GraficoDeSetoresController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public GraficoDeSetoresController(IMapper mapper,
                                          IStockService stockService,
                                          DatabaseContext databaseContext)
        {
            _mapper = mapper;
            _stockService = stockService;
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public async Task<ActionResult<GraficoDeSetoresDTO>> Get(TiposDeInvestimento tipoDeInvestimento = TiposDeInvestimento.Acao)
        {
            GraficoDeSetoresDTO dadosDosSetores = await _stockService.RecuperarDadosDoGraficoDeSetores(tipoDeInvestimento);
            return dadosDosSetores;
        }
    }
}