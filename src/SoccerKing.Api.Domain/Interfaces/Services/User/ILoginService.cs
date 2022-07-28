using SoccerKing.Api.Domain.Entities;
using System.Threading.Tasks;

namespace SoccerKing.Api.Domain.Interfaces.Services.User
{
    public interface ILoginService
    {
        Task<object> FindByLogin(UserEntity user);
    }
}
