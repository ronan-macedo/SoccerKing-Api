using Microsoft.AspNetCore.Mvc;
using Moq;
using SoccerKing.Api.Application.Controllers;
using SoccerKing.Api.Domain.Interfaces.Services.User;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SoccerKing.Api.Application.Test.User.WhenRequestingDelete
{
    public class ReturnDelete
    {
        private UsersController _controller;

        [Fact(DisplayName = "Is possible to peform delete method")]
        public async Task IsPossible_Invoke_Controller_Delete()
        {
            Mock<IUserService> serviceMock = new();
            // Mocar
            serviceMock.Setup(m => m.Delete(It.IsAny<Guid>())).ReturnsAsync(true);

            _controller = new UsersController(serviceMock.Object);

            // Mocar a url da requisição
            Mock<IUrlHelper> url = new();
            url.Setup(u => u.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5500");
            _controller.Url = url.Object;

            ActionResult result = await _controller.Delete(Guid.NewGuid());
            Assert.True(result is OkObjectResult);

            object resultValue = ((OkObjectResult)result).Value;
            Assert.NotNull(resultValue);
            Assert.True((bool)resultValue);
        }
    }
}
