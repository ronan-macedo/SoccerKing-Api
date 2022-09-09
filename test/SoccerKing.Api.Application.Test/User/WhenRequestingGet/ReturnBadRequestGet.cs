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
    public class ReturnBadRequestGet
    {
        private UsersController _controller;

        [Fact(DisplayName = "Is possible to peform get method with bad request")]
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
            _controller.ModelState.AddModelError("Id", "Formato inválido");

            // Mocar a url da requisição
            Mock<IUrlHelper> url = new();
            url.Setup(u => u.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5500");
            _controller.Url = url.Object;

            ActionResult result = await _controller.Delete(Guid.NewGuid());
            Assert.True(result is BadRequestObjectResult);
        }
    }
}
