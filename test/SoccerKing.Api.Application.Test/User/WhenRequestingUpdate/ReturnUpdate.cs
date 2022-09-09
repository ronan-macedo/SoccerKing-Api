using Microsoft.AspNetCore.Mvc;
using Moq;
using SoccerKing.Api.Application.Controllers;
using SoccerKing.Api.Domain.Dtos.User;
using SoccerKing.Api.Domain.Interfaces.Services.User;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SoccerKing.Api.Application.Test.User.WhenRequestingUpdate
{
    public class ReturnUpdate
    {
        private UsersController _controller;

        [Fact(DisplayName = "Is possible to peform update method")]
        public async Task IsPossible_Invoke_Controller_Put()
        {
            Mock<IUserService> serviceMock = new();
            string name = Faker.Name.FullName();
            string email = Faker.Internet.Email();
            string password = BCrypt.Net.BCrypt.HashPassword(Faker.RandomNumber.Next().ToString());

            // Mocar
            serviceMock.Setup(m => m.Put(It.IsAny<UserDtoUpdate>())).ReturnsAsync(
                new UserDtoUpdateResult
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Email = email,
                    UpdateAt = DateTime.UtcNow
                }
            );

            _controller = new UsersController(serviceMock.Object);

            // Mocar a url da requisição
            Mock<IUrlHelper> url = new();
            url.Setup(u => u.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5500");
            _controller.Url = url.Object;

            UserDtoUpdate userDtoUpdate = new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                Password = password
            };

            ActionResult result = await _controller.Put(userDtoUpdate);
            Assert.True(result is OkObjectResult);

            UserDtoUpdateResult resultDto = ((OkObjectResult)result).Value as UserDtoUpdateResult;
            Assert.NotNull(resultDto);
            Assert.Equal(userDtoUpdate.Name, resultDto.Name);
            Assert.Equal(userDtoUpdate.Email, resultDto.Email);
        }
    }
}
