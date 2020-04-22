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
    public class AcompanhamentosController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public AcompanhamentosController(IMapper mapper,
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
            var Acompanhamentos = await _stockService.RecuperarAcompanhamentos();
            var result = Acompanhamentos.Select(r => new
            {
                r.Id,
                codigoDoAtivo = r.Ativo.Codigo,
                nomeDoAtivo = r.Ativo.Nome,
                r.Ativo.PrecoAtual,
                r.Ativo.PrecoAnterior,
                r.PrecoDeCompra,
                comprar = r.Ativo.PrecoAtual.HasValue && (r.Ativo.PrecoAtual.Value <= (r.PrecoDeCompra + 0.5m ))
            })
            .OrderBy(o => o.codigoDoAtivo);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Acompanhamento>> Get(int id)
        {
            try
            {
                var Acompanhamento = await _stockService.RecuperarAcompanhamento(id);

                return Acompanhamento;
            }
            catch
            {
                if (!_databaseContext.Acompanhamentos.Any(e => e.Id == id))
                {
                    return NotFound();
                }

                if (Debugger.IsAttached)
                {
                    throw;
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, AcompanhamentoDTO AcompanhamentoRequest)
        {
            if (id != AcompanhamentoRequest.Id)
            {
                return BadRequest();
            }

            try
            {
                await _stockService.AtualizarAcompanhamento(AcompanhamentoRequest);
            }
            catch
            {
                if (!_databaseContext.Acompanhamentos.Any(e => e.Id == id))
                {
                    return NotFound();
                }

                if (Debugger.IsAttached)
                {
                    throw;
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Acompanhamento>> Post(AcompanhamentoDTO AcompanhamentoRequest)
        {
            var Acompanhamento = await _stockService.IncluirAcompanhamento(AcompanhamentoRequest);
            return CreatedAtAction("Get", new { id = Acompanhamento.Id }, new { id = Acompanhamento.Id, statusCode = StatusCodes.Status201Created });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Acompanhamento>> Delete(int id)
        {
            try
            {
                await _stockService.ExcluirAcompanhamento(id);
            }
            catch
            {
                if (!_databaseContext.Acompanhamentos.Any(e => e.Id == id))
                {
                    return NotFound();
                }

                if (Debugger.IsAttached)
                {
                    throw;
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }
    }
}