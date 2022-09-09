using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SoccerKing.Api.CrossCutting.DependencyInjection;
using SoccerKing.Api.CrossCutting.Mappings;
using SoccerKing.Api.Data.Context;
using SoccerKing.Api.Domain.Security;
using System;
using System.Collections.Generic;

namespace SoccerKing.Api.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _enviroment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment _enviroment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_enviroment.IsEnvironment("Test"))
            {
                Environment.SetEnvironmentVariable("POSTGRES_CONNECTION", "Host=localhost;Database=soccerdb_integration;Username=postgres;Password=S3nha#2021");
                Environment.SetEnvironmentVariable("DB_POSTGRES", "SQLSERVER");
                Environment.SetEnvironmentVariable("MIGRATION", "InitialMigration");
                Environment.SetEnvironmentVariable("Audience", "company@email.com");
                Environment.SetEnvironmentVariable("Issuer", "companyIssuer");
                Environment.SetEnvironmentVariable("Seconds", "1800");
            }

            // Realiza injeção de depedência da camada de serviços e dados
            ConfigureService.ConfigureDependencesService(services);
            ConfigureRepository.ConfigureDependencesRepository(services);

            /**
             * Configura os profiles que serão utilizados pelo 
             * AutoMapper para o mapeamento de entidades, Dtos, 
             * e models
             */
            MapperConfiguration config = new(conf =>
            {
                conf.AddProfile(new DtoToModelProfile());
                conf.AddProfile(new EntityToDtoProfile());
                conf.AddProfile(new ModelToEntityProfile());
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            #region Configurações do Token
            // Configurações para geração da chave do token
            SigningConfiguration signingConfiguration = new();
            services.AddSingleton(signingConfiguration);

            // Configuração do token (Audience, Issuer e Seconds)
            TokenConfiguration tokenConfiguration = new()
            { 
                Audience = Environment.GetEnvironmentVariable("Audience"),
                Issuer = Environment.GetEnvironmentVariable("Issuer"),
                Seconds = int.Parse(Environment.GetEnvironmentVariable("Seconds"))
            };            
            services.AddSingleton(tokenConfiguration);

            /**
             * Realiza o processo de autenticação do token recebido
             * e configura quais parâmetros serão avaliados
             */
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                TokenValidationParameters paramsValidation = bearerOptions.TokenValidationParameters;
                // Vakuda a assinatura de um token
                paramsValidation.IssuerSigningKey = signingConfiguration.Key;
                paramsValidation.ValidAudience = tokenConfiguration.Audience;
                paramsValidation.ValidIssuer = tokenConfiguration.Issuer;
                paramsValidation.ValidateLifetime = true;
                // Verifica se o token ainda é valido
                paramsValidation.ValidateIssuerSigningKey = true;
                // Tempo de tolerância para expiração de um token
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            /**
             * Ativa o uso do token como forma de autorizar o acesso
             * a recursos do projeto
             */
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
            #endregion

            services.AddControllers();

            /**
             * Habilitar qualquer origem da requisição, qualquer header 
             * e qualquer método 
             * (remover ao utilizar em ambiente prod)
             */
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            #region Configurações do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SoccerKing API",
                    Description = "Domain Driven Design Application",
                    Version = "v 1.0.0"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Enter the JWT Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SoccerKing API"));
            }

            app.UseRouting();

            /**
             * Habilitar qualquer origem da requisição, qualquer header 
             * e qualquer método 
             * (remover ao utilizar em ambiente prod)
             */
            app.UseCors("AllowOrigin");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            // Atualiza (se necessário) ou inicializa a base de dados              
            if (Environment.GetEnvironmentVariable("MIGRATION").ToLower() == "InitialMigration".ToLower())
            {
                using (var service = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    using (var context = service.ServiceProvider.GetService<MyDbContext>())
                    {                        
                        context.Database.Migrate();
                    }
                }
            }
        }
    }
}
