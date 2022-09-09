using Moq;
using SoccerKing.Api.Domain.Dtos.User;
using SoccerKing.Api.Domain.Interfaces.Services.User;
using System.Threading.Tasks;
using Xunit;

namespace SoccerKing.Api.Service.Test.User
{
    public class ExecuteGet : UserTest
    {
        private IUserService _service;
        private Mock<IUserService> _serviceMock;

        [Fact(DisplayName ="Is possible execute get method")]
        public async Task IsPossible_Execute_Get()
        {
            _serviceMock = new();
            _serviceMock.Setup(m => m.Get(UserId)).ReturnsAsync(userDto);
            _service = _serviceMock.Object;

            UserDto result = await _service.Get(UserId);
            Assert.NotNull(result);
            Assert.Equal(UserId, result.Id);
            Assert.Equal(UserName, result.Name);
        }
    }
}
