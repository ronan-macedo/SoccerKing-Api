using SoccerKing.Api.Domain.Entities;
using SoccerKing.Api.Domain.Interfaces.Services.User;
using SoccerKing.Api.Domain.Repository;
using System;
using System.Threading.Tasks;

namespace SoccerKing.Api.Service.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _repository;

        public LoginService(ILoginRepository repository)
        {
            _repository = repository;
        }

        public async Task<object> FindByLogin(UserEntity user)
        {            
            if (user == null || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
                return null;

            UserEntity baseUser = await _repository.FindByLogin(user.Email, user.Password);

            if (baseUser == null)
                return null;
            
            return baseUser;
        }
    }
}
