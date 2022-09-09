using Microsoft.AspNetCore.Mvc;
using Moq;
using SoccerKing.Api.Application.Controllers;
using SoccerKing.Api.Domain.Dtos.User;
using SoccerKing.Api.Domain.Interfaces.Services.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SoccerKing.Api.Application.Test.User.WhenRequestingGet
{
    public class ReturnBadRequestGetAll
    {
        private UsersController _controller;

        [Fact(DisplayName = "Is possible to peform get method with bad request")]
        public async Task IsPossible_Invoke_Controller_Get()
        {
            Mock<IUserService> serviceMock = new();
            List<UserDto> userDtos = new();

            for (int i = 0; i < 10; i++)
            {
                UserDto user = new()
                {
                    Id = Guid.NewGuid(),
                    Name = Faker.Name.FullName(),
                    Email = Faker.Internet.Email(),
                    CreateAt = DateTime.UtcNow
                };

                userDtos.Add(user);
            }

            // Mocar
            serviceMock.Setup(m => m.GetAll()).ReturnsAsync(userDtos);

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
