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
    public class AtivosController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public AtivosController(IMapper mapper,
                                 IStockService stockService, 
                                 DatabaseContext databaseContext)
        {
            _mapper = mapper;
            _stockService = stockService;
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ativo>>> Get()
        {
            var Ativos = await _stockService.RecuperarAtivos();

            return Ativos.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ativo>> Get(int id)
        {                
            try
            {
                var Ativo = await _stockService.RecuperarAtivo(id);

                return Ativo;
            }
            catch  
            {
                if (!_databaseContext.Ativos.Any(e => e.Id == id))
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

        [HttpPost]
        public async Task<ActionResult> Refresh()
        {
            await _stockService.CarregarCotacoes();

            return Ok();
        }
    }
}