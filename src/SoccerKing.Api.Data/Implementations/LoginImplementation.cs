using Microsoft.EntityFrameworkCore;
using SoccerKing.Api.Data.Context;
using SoccerKing.Api.Data.Repository;
using SoccerKing.Api.Domain.Entities;
using SoccerKing.Api.Domain.Repository;
using System;
using System.Threading.Tasks;

namespace SoccerKing.Api.Data.Implementations
{
    public class LoginImplementation : BaseRepository<UserEntity>, ILoginRepository
    {
        private readonly DbSet<UserEntity> _dbSet;
        public LoginImplementation(MyDbContext dbContext) : base(dbContext)
        {
            _dbSet = dbContext.Set<UserEntity>();
        }
        public async Task<UserEntity> FindByLogin(string email, string password)
        {
            try
            {
                UserEntity result = await _dbSet.SingleOrDefaultAsync(u => u.Email.Equals(email));

                if (result == null)
                    return null;

                if (!BCrypt.Net.BCrypt.Verify(password, result.Password))
                    return null;

                return result;
            }
            catch (Exception)
            {
                return null;
            }            
        }
    }
}
