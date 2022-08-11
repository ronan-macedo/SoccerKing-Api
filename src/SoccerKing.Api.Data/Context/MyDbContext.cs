using Microsoft.EntityFrameworkCore;
using SoccerKing.Api.Data.Mapping;
using SoccerKing.Api.Domain.Entities;
using System;

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

            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    Email = "admin@email.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123"),
                    CreateAt = DateTime.UtcNow,
                    UpdateAt = null
                }
            );
        }
    }
}
