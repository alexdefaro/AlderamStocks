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
    public class AuthenticationController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        private readonly IStockService _stockService;
        private readonly ITokenService _tokenService;

        public AuthenticationController(IMapper mapper,
                                        IStockService stockService,
                                        DatabaseContext databaseContext,
                                        ITokenService tokenService)
        {
            _mapper = mapper;
            _stockService = stockService;
            _databaseContext = databaseContext;
            _tokenService = tokenService;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Post(LoginViewModel loginViewModel)
        {
            if (loginViewModel.UserKey == "Colt" || loginViewModel.UserKey == "Invest")
            {
                var jwtToken = _tokenService.GenerateJWTToken(loginViewModel.UserKey);
                await _stockService.CarregarCotacoesHG();

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