using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SoccerKing.Api.Data.Context;
using System;

namespace SoccerKing.Api.Data.Test
{
    public abstract class BaseTestData
    {
        public BaseTestData()
        {

        }

        public class DbTest : IDisposable
        {
            private readonly string dataBaseName = $"soccerdbtest_{Guid.NewGuid().ToString().Replace("-", string.Empty)}";
            public ServiceProvider ServiceProvider { get; private set; }

            public DbTest()
            {
                ServiceCollection service = new();
                service.AddDbContext<MyDbContext>(db =>
                    db.UseNpgsql($"Host=localhost;Database={dataBaseName};Username=postgres;Password=S3nha#2021")
                    , ServiceLifetime.Transient);

                ServiceProvider = service.BuildServiceProvider();
                using MyDbContext context = ServiceProvider.GetService<MyDbContext>();
                context.Database.EnsureCreated();
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    using MyDbContext context = ServiceProvider.GetService<MyDbContext>();
                    context.Database.EnsureDeleted();
                }
            }
        }
    }
}
