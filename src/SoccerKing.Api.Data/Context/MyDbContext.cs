using Microsoft.EntityFrameworkCore;
using SoccerKing.Api.Data.Mapping;
using SoccerKing.Api.Domain.Entities;

namespace SoccerKing.Api.Data.Context
{
    public class MyDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>(new UserMap().Configure);
        }
    }
}
