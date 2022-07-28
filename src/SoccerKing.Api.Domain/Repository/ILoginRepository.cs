using SoccerKing.Api.Domain.Entities;
using SoccerKing.Api.Domain.Interfaces;
using System.Threading.Tasks;

namespace SoccerKing.Api.Domain.Repository
{
    public interface ILoginRepository : IRepository<UserEntity>
    {
        Task<UserEntity> FindByLogin(string email, string password);
    }
}
