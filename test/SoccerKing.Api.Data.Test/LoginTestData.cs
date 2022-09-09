using Microsoft.Extensions.DependencyInjection;
using SoccerKing.Api.Data.Context;
using SoccerKing.Api.Data.Implementations;
using SoccerKing.Api.Domain.Entities;
using System.Threading.Tasks;
using Xunit;

namespace SoccerKing.Api.Data.Test
{
    public class LoginTestData : BaseTestData, IClassFixture<BaseTestData.DbTest>
    {
        private readonly ServiceProvider _service;

        public LoginTestData(DbTest dbTest)
        {
            _service = dbTest.ServiceProvider;
        }

        [Fact(DisplayName = "Login User")]
        [Trait("Login", "LoginUser")]
        public async Task IsPossible_Login()
        {
            using MyDbContext context = _service.GetService<MyDbContext>();
            LoginImplementation loginImplementation = new(context);

            UserEntity login = await loginImplementation.FindByLogin("admin@email.com", "123");
            Assert.NotNull(login);
            Assert.Equal("Admin", login.Name);
            Assert.Equal("admin@email.com", login.Email);
        }
    }
}
