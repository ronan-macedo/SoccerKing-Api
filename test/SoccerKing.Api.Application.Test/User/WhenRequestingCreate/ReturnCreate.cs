using Microsoft.AspNetCore.Mvc;
using Moq;
using SoccerKing.Api.Application.Controllers;
using SoccerKing.Api.Domain.Dtos.User;
using SoccerKing.Api.Domain.Interfaces.Services.User;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SoccerKing.Api.Application.Test.User.WhenRequestingCreate
{
    public class ReturnCreate
    {
        private UsersController _controller;

        [Fact(DisplayName = "Is possible to peform create method")]
        public async Task IsPossible_Invoke_Controller_Post()
        {
            Mock<IUserService> serviceMock = new();
            string name = Faker.Name.FullName();
            string email = Faker.Internet.Email();
            string password = BCrypt.Net.BCrypt.HashPassword(Faker.RandomNumber.Next().ToString());

            // Mocar
            serviceMock.Setup(m => m.Post(It.IsAny<UserDtoCreate>())).ReturnsAsync(
                new UserDtoCreateResult
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

            UserDtoCreate userDtoCreate = new()
            {
                Name = name,
                Email = email,
                Password = password
            };

            ActionResult result = await _controller.Post(userDtoCreate);
            Assert.True(result is CreatedResult);

            UserDtoCreateResult resultDto = ((CreatedResult)result).Value as UserDtoCreateResult;
            Assert.NotNull(resultDto);
            Assert.Equal(userDtoCreate.Name, resultDto.Name);
            Assert.Equal(userDtoCreate.Email, resultDto.Email);
        }
    }
}
