using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using alderam.stocks.api.Database;
using alderam.stocks.api.Models;

namespace alderam.stocks.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AtivosController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public AtivosController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Ativoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ativo>>> GetAtivos()
        {
            return await _context.Ativos.ToListAsync();
        }

        // GET: api/Ativoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ativo>> GetAtivo(int id)
        {
            var ativo = await _context.Ativos.FindAsync(id);

            if (ativo == null)
            {
                return NotFound();
            }

            return ativo;
        }

        // PUT: api/Ativoes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAtivo(int id, Ativo ativo)
        {
            if (id != ativo.Id)
            {
                return BadRequest();
            }

            _context.Entry(ativo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AtivoExists(id))
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

        // POST: api/Ativoes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Ativo>> PostAtivo(Ativo ativo)
        {
            _context.Ativos.Add(ativo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAtivo", new { id = ativo.Id }, ativo);
        }

        // DELETE: api/Ativoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ativo>> DeleteAtivo(int id)
        {
            var ativo = await _context.Ativos.FindAsync(id);
            if (ativo == null)
            {
                return NotFound();
            }

            _context.Ativos.Remove(ativo);
            await _context.SaveChangesAsync();

            return ativo;
        }

        private bool AtivoExists(int id)
        {
            return _context.Ativos.Any(e => e.Id == id);
        }
    }
}
