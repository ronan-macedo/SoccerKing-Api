using Moq;
using SoccerKing.Api.Domain.Dtos.User;
using SoccerKing.Api.Domain.Interfaces.Services.User;
using System.Threading.Tasks;
using Xunit;

namespace SoccerKing.Api.Service.Test.User
{
    public class ExecutePost : UserTest
    {
        private IUserService _service;
        private Mock<IUserService> _serviceMock;

        [Fact(DisplayName = "Is possible execute post method")]
        public async Task IsPossible_Execute_Post()
        {
            _serviceMock = new();
            _serviceMock.Setup(m => m.Post(userDtoCreate)).ReturnsAsync(userDtoCreateResult);
            _service = _serviceMock.Object;

            UserDtoCreateResult result = await _service.Post(userDtoCreate);
            Assert.NotNull(result);
            Assert.Equal(UserName, result.Name);
            Assert.Equal(UserEmail, result.Email);
        }
    }
}
