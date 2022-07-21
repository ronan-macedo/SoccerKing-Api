using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SoccerKing.Api.Data.Context
{
    public class DbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            // Criar as migrações            
            string connectionString = "Server=localhost, 1433;Initial Catalog=soccerdb;User ID=SA;Password=S3nha#2021;";
            var optionBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionBuilder.UseSqlServer(connectionString);
            return new MyDbContext(optionBuilder.Options);
        }
    }
}
