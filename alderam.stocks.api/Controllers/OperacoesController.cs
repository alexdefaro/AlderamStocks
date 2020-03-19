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

namespace alderam.stocks.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperacoesController : ControllerBase
    {
        private readonly ILogger<OperacoesController> _logger;
        private readonly DatabaseContext _databaseContext;

        public OperacoesController(ILogger<OperacoesController> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Operacao>>> Get()
        {
            return await _databaseContext.Operacoes.ToListAsync();
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
        public async Task<ActionResult<Operacao>> Post(Operacao operacao)
        {
            _databaseContext.Operacoes.Add(operacao);
            await _databaseContext.SaveChangesAsync();

            return CreatedAtAction("GetOperacao", new { id = operacao.Id }, operacao);
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
