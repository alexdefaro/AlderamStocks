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
    public class AuthenticationController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public AuthenticationController(IMapper mapper,
                                 DatabaseContext databaseContext)
        {
            _mapper = mapper;
            _databaseContext = databaseContext;
        }

        [HttpPost]
        public ActionResult Post(LoginViewModel loginViewModel)
        {
            if (loginViewModel.UserKey == "Colt")
            {
                return Ok();
            }

            return Unauthorized();
        }
    }

    public class LoginViewModel
    {
        public string UserKey { get; set; }
    }
}