using SoccerKing.Api.Domain.Dtos;
using System.Threading.Tasks;

namespace SoccerKing.Api.Domain.Interfaces.Services.User
{
    public interface ILoginService
    {
        Task<object> FindByLogin(LoginDto login);
    }
}
