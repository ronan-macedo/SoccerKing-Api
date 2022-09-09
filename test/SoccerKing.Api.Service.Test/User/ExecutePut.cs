using Moq;
using SoccerKing.Api.Domain.Dtos.User;
using SoccerKing.Api.Domain.Interfaces.Services.User;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SoccerKing.Api.Service.Test.User
{
    public class ExecutePut : UserTest
    {
        private IUserService _service;
        private Mock<IUserService> _serviceMock;

        [Fact(DisplayName = "Is possible execute put method")]
        public async Task IsPossible_Execute_Put()
        {
            _serviceMock = new();
            _serviceMock.Setup(m => m.Put(userDtoUpdate)).ReturnsAsync(userDtoUpdateResult);
            _service = _serviceMock.Object;

            UserDtoUpdateResult result = await _service.Put(userDtoUpdate);
            Assert.NotNull(result);
            Assert.Equal(UserNameUpdate, result.Name);
            Assert.Equal(UserEmailUpdate, result.Email);
            Assert.True(result.UpdateAt != new DateTime());
        }
    }
}
