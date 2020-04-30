using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using alderam.stocks.api.Database;
using alderam.stocks.api.Models;
using AutoMapper;
using alderam.stocks.api.Services;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace alderam.stocks.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SetoresController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public SetoresController(IMapper mapper,
                                 IStockService stockService, 
                                 DatabaseContext databaseContext)
        {
            _mapper = mapper;
            _stockService = stockService;
            _databaseContext = databaseContext;
        }

        [HttpGet]
        [ResponseCache(Duration = 240)]
        public async Task<ActionResult<IEnumerable<Setor>>> Get()
        {
            var Setores = await _stockService.RecuperarSetores();

            return Setores.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Setor>> Get(int id)
        {                
            try
            {
                var Setor = await _stockService.RecuperarSetor(id);

                return Setor;
            }
            catch  
            {
                if (!_databaseContext.Setores.Any(e => e.Id == id))
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
    }
}