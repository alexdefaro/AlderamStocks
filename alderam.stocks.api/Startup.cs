using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using AutoMapper;

using alderam.stocks.api.Database;
using alderam.stocks.api.Services;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;

namespace alderam.stocks.api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public static readonly ILoggerFactory _loggerFactory
            = LoggerFactory.Create(builder =>
                {
                    builder
                        .AddFilter((category, level) =>
                            category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                        .AddConsole();
                });

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(c =>
            {
                c.AddDefaultPolicy(options => options.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
                options.UseLoggerFactory(_loggerFactory);
                options.EnableSensitiveDataLogging(_webHostEnvironment.IsDevelopment());
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IStockService, StockService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressMapClientErrors = true;
                });

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.PropertyNamingPolicy = null
            );


            var jwtPrivateKey = Encoding.ASCII.GetBytes(Configuration["Secrets:jwt_private_key"]);

            services.AddAuthentication(a =>
                {
                    a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            )
            .AddJwtBearer(a =>
                {
                    a.RequireHttpsMetadata = false;
                    a.SaveToken = true;
                    a.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(jwtPrivateKey),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                }
            );

            //services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Alderam.Stocks.v1",
                    Description = "Web API for Alderam.Stocks",
                    TermsOfService = new Uri("https://alderam.stocks.termsofservice.com"),
                    
                    Contact = new OpenApiContact
                    {
                        Name = "Developers contact",
                        Email = string.Empty,
                        Url = new Uri("https://alderam.stocks.developers.com"),
                    },

                    License = new OpenApiLicense
                    {
                        Name = "License page",
                        Url = new Uri("https://alderam.stocks.license.com"),
                    }
                });
            });

        }

        public void Configure(IApplicationBuilder app)
        {
            if (_webHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}


