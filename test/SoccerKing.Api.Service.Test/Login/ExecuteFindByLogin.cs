using Moq;
using SoccerKing.Api.Domain.Dtos;
using SoccerKing.Api.Domain.Interfaces.Services.User;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SoccerKing.Api.Service.Test.Login
{
    public class ExecuteFindByLogin
    {
        private ILoginService _service;
        private Mock<ILoginService> _serviceMock;

        [Fact(DisplayName = "Is possible execute FindByLogin method")]
        public async Task Is_Possible_Execute_FindByLogin()
        {
            string email = Faker.Internet.Email();
            string password = BCrypt.Net.BCrypt.HashPassword(Faker.RandomNumber.Next().ToString());

            object returnObj = new
            {
                authenticated = true,
                created = DateTime.UtcNow,
                expiration = DateTime.UtcNow.AddHours(8),
                accessToken = Guid.NewGuid(),
                userName = Faker.Name.FullName(),
                userEmail = email,
                message = "Usuário logado com sucesso"
            };

            LoginDto login = new()
            {
                Email = email,
                Password = password
            };

            _serviceMock = new Mock<ILoginService>();
            _serviceMock.Setup(m => m.FindByLogin(login)).ReturnsAsync(returnObj);
            _service = _serviceMock.Object;

            object result = await _service.FindByLogin(login);
            Assert.NotNull(result);
        }
    }
}
