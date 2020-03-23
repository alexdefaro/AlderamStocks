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

namespace alderam.stocks.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoletasController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public BoletasController(IMapper mapper,
                                 IStockService stockService, 
                                 DatabaseContext databaseContext)
        {
            _mapper = mapper;
            _stockService = stockService;
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Boleta>>> Get()
        {
            return await _databaseContext.Boletas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Boleta>> Get(int id)
        {
             var boleta = await _databaseContext.Boletas.FindAsync(id);

            if (boleta == null)
            {
                return NotFound();
            }

            return boleta;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, BoletaDTO boletaRequest)
        {
            if (id != boletaRequest.Id)
            {
                return BadRequest();
            }

            _databaseContext.Entry(boletaRequest).State = EntityState.Modified;

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_databaseContext.Boletas.Any(e => e.Id == id))
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
        public async Task<ActionResult<Boleta>> Post(BoletaDTO boletaRequest)
        {
            var boleta = await _stockService.IncluirBoleta(boletaRequest);
            return CreatedAtAction("Get", new { id = boleta.Id }, boleta);
            //return CreatedAtAction("Get", new { id = boleta.Id }, new { id = boleta.Id, statusCode = StatusCodes.Status201Created });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Boleta>> Delete(int id)
        {
            var boleta = await _databaseContext.Boletas.FindAsync(id);
            if (boleta == null)
            {
                return NotFound();
            }

            _databaseContext.Boletas.Remove(boleta);
            await _databaseContext.SaveChangesAsync();

            return boleta;
        }
    }
}