using SoccerKing.Api.Domain.Entities;
using SoccerKing.Api.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerKing.Api.Domain.Repository
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<UserEntity> UserInsert(UserEntity user);
        Task<UserEntity> UserUpdate(UserEntity user);
    }
}
