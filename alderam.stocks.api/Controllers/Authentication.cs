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
using Microsoft.Extensions.Configuration;

namespace alderam.stocks.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        private readonly IStockService _stockService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IMapper mapper,
                                        IStockService stockService,
                                        DatabaseContext databaseContext,
                                        ITokenService tokenService,
                                        IConfiguration configuration)
        {
            _mapper = mapper;
            _stockService = stockService;
            _databaseContext = databaseContext;
            _tokenService = tokenService;
            _configuration = configuration;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Post(LoginViewModel loginViewModel)
        {
            var allowedKeys = _configuration["Auth:AllowedKeys"]?.Split(',') ?? Array.Empty<string>();
            if (allowedKeys.Contains(loginViewModel.UserKey))
            {
                var jwtToken = _tokenService.GenerateJWTToken(loginViewModel.UserKey);

                //await _stockService.CarregarCotacoes();

                return Ok(new { 
                    userKey = loginViewModel.UserKey, 
                    token = jwtToken 
                });
            }

            return Unauthorized();
        }
    }

    public class LoginViewModel
    {
        public string UserKey { get; set; }
    }
}