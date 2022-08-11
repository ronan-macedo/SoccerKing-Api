using Microsoft.Extensions.DependencyInjection;
using SoccerKing.Api.Data.Context;
using SoccerKing.Api.Data.Implementations;
using SoccerKing.Api.Domain.Entities;
using System.Threading.Tasks;
using Xunit;

namespace SoccerKing.Api.Data.Test
{
    public class Login : BaseTestData, IClassFixture<BaseTestData.DbTest>
    {
        private readonly ServiceProvider _service;

        public Login(DbTest dbTest)
        {
            _service = dbTest.ServiceProvider;
        }        

        [Fact(DisplayName = "Login")]
        [Trait("Login", "Login")]
        public async Task IsPossible_Login()
        {
            using MyDbContext context = _service.GetService<MyDbContext>();
            LoginImplementation _loginImplementation = new(context);

            UserEntity _login = await _loginImplementation.FindByLogin("admin@email.com", "123");
            Assert.NotNull(_login);
            Assert.Equal("Admin", _login.Name);
            Assert.Equal("admin@email.com", _login.Email);
        }
    }
}
