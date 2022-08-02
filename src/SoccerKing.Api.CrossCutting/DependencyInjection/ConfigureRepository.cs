﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoccerKing.Api.Data.Context;
using SoccerKing.Api.Data.Implementations;
using SoccerKing.Api.Data.Repository;
using SoccerKing.Api.Domain.Interfaces;
using SoccerKing.Api.Domain.Repository;

namespace SoccerKing.Api.CrossCutting.DependencyInjection
{
    public static class ConfigureRepository
    {        
        public static void ConfigureDependencesRepository(IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<MyDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("MyConn"))
                );

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ILoginRepository, LoginImplementation>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
