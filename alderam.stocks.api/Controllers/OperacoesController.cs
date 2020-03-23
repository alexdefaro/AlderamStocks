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
using System.Runtime.Serialization;
using alderam.stocks.api.Models.DTOs;

namespace alderam.stocks.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperacoesController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly ILogger<OperacoesController> _logger;
        private readonly DatabaseContext _databaseContext;

        public IMapper _mapper { get; }

        public OperacoesController(IMapper mapper,
                                   IStockService stockService, 
                                   ILogger<OperacoesController> logger,
                                   DatabaseContext databaseContext)
        {
            _mapper = mapper;
            _stockService = stockService;
            _logger = logger;
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Operacao>>> Get()
        {
            return await _databaseContext.Operacoes
                .Include(i => i.Ativo)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Operacao>> Get(int id)
        {
            var operacao = await _databaseContext.Operacoes.FindAsync(id);

            if (operacao == null)
            {
                return NotFound();
            }

            return operacao;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Operacao operacao)
        {
            if (id != operacao.Id)
            {
                return BadRequest();
            }

            _databaseContext.Entry(operacao).State = EntityState.Modified;

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_databaseContext.Operacoes.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Operacao>> Post(OperacaoDTO operacaoRequest)
        {
            var ativo = _databaseContext.Ativos.SingleOrDefault(r => r.Codigo == operacaoRequest.CodigoDoAtivo);

            if (ativo == null)
            {
                ativo = new Ativo()
                {
                    Codigo = operacaoRequest.CodigoDoAtivo,
                    Nome = operacaoRequest.CodigoDoAtivo,
                    DataDeCriacao = DateTime.Now
                };
            }

            var operacao = new Operacao();

            operacao = _mapper.Map<Operacao>(operacaoRequest);
            operacao.Ativo = ativo;
            operacao.DataDeCriacao = DateTime.Now;
            operacao.ValorDaOperacao = (operacaoRequest.Quantitidade * operacaoRequest.PrecoDeCompra); 

            _databaseContext.Operacoes.Add(operacao);
            await _databaseContext.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = operacao.Id }, operacao);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Operacao>> Delete(int id)
        {
            var operacao = await _databaseContext.Operacoes.FindAsync(id);
            if (operacao == null)
            {
                return NotFound();
            }

            _databaseContext.Operacoes.Remove(operacao);
            await _databaseContext.SaveChangesAsync();

            return operacao;
        }
    }
}
