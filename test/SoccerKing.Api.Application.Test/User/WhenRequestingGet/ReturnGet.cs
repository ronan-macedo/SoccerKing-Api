using Microsoft.AspNetCore.Mvc;
using Moq;
using SoccerKing.Api.Application.Controllers;
using SoccerKing.Api.Domain.Dtos.User;
using SoccerKing.Api.Domain.Interfaces.Services.User;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SoccerKing.Api.Application.Test.User.WhenRequestingGet
{
    public class ReturnGet
    {
        private UsersController _controller;

        [Fact(DisplayName = "Is possible to peform get method")]
        public async Task IsPossible_Invoke_Controller_Get()
        {
            Mock<IUserService> serviceMock = new();
            string name = Faker.Name.FullName();
            string email = Faker.Internet.Email();

            // Mocar
            serviceMock.Setup(m => m.Get(It.IsAny<Guid>())).ReturnsAsync(
                new UserDto
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Email = email,
                    CreateAt = DateTime.UtcNow
                }
            );

            _controller = new UsersController(serviceMock.Object);

            // Mocar a url da requisição
            Mock<IUrlHelper> url = new();
            url.Setup(u => u.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5500");
            _controller.Url = url.Object;

            ActionResult result = await _controller.Get(Guid.NewGuid());
            Assert.True(result is OkObjectResult);

            UserDto resultValue = ((OkObjectResult)result).Value as UserDto;
            Assert.NotNull(resultValue);
            Assert.Equal(name, resultValue.Name);
            Assert.Equal(email, resultValue.Email);
        }
    }
}
