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
            var boletas = await _stockService.RecuperarBoletas();

            return boletas.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Boleta>> Get(int id)
        {                
            try
            {
                var boleta = await _stockService.RecuperarBoleta(id);

                return boleta;
            }
            catch  
            {
                if (!_databaseContext.Boletas.Any(e => e.Id == id))
                {
                    return NotFound();
                }

                if (Debugger.IsAttached)
                {
                    throw;
                }                

                return  StatusCode(StatusCodes.Status500InternalServerError);
            }            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, BoletaDTO boletaRequest)
        {
            if (id != boletaRequest.Id)
            {
                return BadRequest();
            }

            try
            {
                await _stockService.AtualizarBoleta(boletaRequest);
            }
            catch  
            {
                if (!_databaseContext.Boletas.Any(e => e.Id == id))
                {
                    return NotFound();
                }

                if (Debugger.IsAttached)
                {
                    throw;
                }                

                return  StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Boleta>> Post(BoletaDTO boletaRequest)
        {
            var boleta = await _stockService.IncluirBoleta(boletaRequest);
            return CreatedAtAction("Get", new { id = boleta.Id }, new { id = boleta.Id, statusCode = StatusCodes.Status201Created });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Boleta>> Delete(int id)
        {
            try
            {
                await _stockService.ExcluirBoleta(id);
            }
            catch  
            {
                if (!_databaseContext.Boletas.Any(e => e.Id == id))
                {
                    return NotFound();
                }

                if (Debugger.IsAttached)
                {
                    throw;
                }                

                return  StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }
    }
}