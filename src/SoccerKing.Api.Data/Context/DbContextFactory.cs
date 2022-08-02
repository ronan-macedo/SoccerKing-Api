using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace SoccerKing.Api.Data.Context
{
    public class DbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            // Criar as migrações
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("datasettings.json")
                .Build();

            DbContextOptionsBuilder<MyDbContext> optionBuilder = new();
            optionBuilder.UseSqlServer(configuration.GetConnectionString("MyConn"));
            return new MyDbContext(optionBuilder.Options);
        }
    }
}
