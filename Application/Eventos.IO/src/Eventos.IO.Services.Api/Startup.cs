using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Eventos.IO.Infra.CrossCutting.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Eventos.IO.Infra.CrossCutting.Identity.Models;
using AutoMapper;
using Eventos.IO.Infra.CrossCutting.IoC;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Eventos.IO.Infra.CrossCutting.Bus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Eventos.IO.Services.Api.Configurations;
using Swashbuckle.AspNetCore.Swagger;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Eventos.IO.Infra.CrossCutting.Identity.Authorization;
using System;
using Elmah.Io.AspNetCore;
using Elmah.Io.Extensions.Logging;

namespace Eventos.IO.Services.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private const string SecretKey = "eventosio@meuambienteToken";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddOptions();
            services.AddMvc(options =>
            {
                options.OutputFormatters.Remove(new XmlDataContractSerializerOutputFormatter());
                options.UseCentralRoutePrefix(new RouteAttribute("api/v{version}"));

                var policy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("PodeLerEventos", policy => policy.RequireClaim(nameof(Eventos), "Ler"));
                options.AddPolicy("PodeGravar", policy => policy.RequireClaim(nameof(Eventos), "Gravar"));
            });

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtTokenOptions));

            services.Configure<JwtTokenOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtTokenOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtTokenOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;

                paramsValidation.ValidateIssuer = true;
                paramsValidation.ValidIssuer = jwtAppSettingOptions[nameof(JwtTokenOptions.Issuer)];

                paramsValidation.ValidateAudience = true;
                paramsValidation.ValidAudience = jwtAppSettingOptions[nameof(JwtTokenOptions.Audience)];

                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.IssuerSigningKey = _signingKey;

                paramsValidation.RequireExpirationTime = true;
                paramsValidation.ValidateLifetime = true;

                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            services.AddAutoMapper();
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Eventos.IO API",
                    Description = "API do site Eventos.IO",
                    TermsOfService = "Nenhum",
                    Contact = new Contact { Name = "Desenvolvedor X", Email = "email@eventos.io", Url = "http://eventos.io" },
                    License = new License { Name = "MIT", Url = "http://eventos.io/licensa" }
                });
            });


            // Registrar todos os DI
            RegisterServices(services);
        }

        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env,
                              ILoggerFactory loggerFactory,
                              IHttpContextAccessor accessor)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var elmahSts = new ElmahIoSettings
            {
                OnMessage = message =>
                {
                    message.Version = "v1.0";
                    message.Application = "Eventos.IO";
                    message.User = accessor.HttpContext.User.Identity.Name;
                },
            };

            loggerFactory.AddElmahIo("e1ce5cbd905b42538c649f6e1d66351e", new Guid("19ad15fd-5158-4b7a-b36d-ab56dfe4500a"));
            app.UseElmahIo("e1ce5cbd905b42538c649f6e1d66351e", new Guid("19ad15fd-5158-4b7a-b36d-ab56dfe4500a"), elmahSts);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            app.UseStaticFiles();
            app.UseMvc();

            //está dando erro??? 
            //app.UseSwaggerAuthorized();
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Eventos.IO API v1.0");
            });

            InMemoryBus.ContainerAccessor = () => accessor.HttpContext.RequestServices;
        }

        private static void RegisterServices(IServiceCollection services)
        {
            NativeInjectorBootStrapper.RegisterServices(services);
        }
    }
}
