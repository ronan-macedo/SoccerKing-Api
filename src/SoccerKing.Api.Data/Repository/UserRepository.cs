using Microsoft.EntityFrameworkCore;
using SoccerKing.Api.Data.Context;
using SoccerKing.Api.Domain.Entities;
using SoccerKing.Api.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerKing.Api.Data.Repository
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        private readonly DbSet<UserEntity> _dbSet;
        public UserRepository(MyDbContext dbContext) : base(dbContext)
        {
            _dbSet = dbContext.Set<UserEntity>();
        }

        public async Task<UserEntity> UserInsert(UserEntity user)
        {
            try
            {
                if (user.Id == Guid.Empty)
                    user.Id = Guid.NewGuid();

                user.CreateAt = DateTime.UtcNow;
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                _dbSet.Add(user);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return null;
            }

            return user;
        }

        public async Task<UserEntity> UserUpdate(UserEntity user)
        {
            try
            {
                UserEntity result = await _dbSet.SingleOrDefaultAsync(p => p.Id.Equals(user.Id));

                if (result == null)
                    return null;

                user.UpdateAt = DateTime.UtcNow;
                user.CreateAt = result.CreateAt;

                if (string.IsNullOrWhiteSpace(user.Password))
                    user.Password = result.Password;

                if (!user.Password.Equals(result.Password))
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                _dbContext.Entry(result).CurrentValues.SetValues(user);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return null;
            }

            return user;
        }
    }
}
