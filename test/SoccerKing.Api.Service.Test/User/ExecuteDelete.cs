using Moq;
using SoccerKing.Api.Domain.Interfaces.Services.User;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SoccerKing.Api.Service.Test.User
{
    public class ExecuteDelete : UserTest
    {
        private IUserService _service;
        private Mock<IUserService> _serviceMock;

        [Fact(DisplayName = "Is possible execute delete method")]
        public async Task IsPossible_Execute_Delete()
        {
            _serviceMock = new();
            _serviceMock.Setup(m => m.Delete(UserId)).ReturnsAsync(true);
            _service = _serviceMock.Object;

            // deletar usuário existente
            bool result = await _service.Delete(UserId);
            Assert.True(result);
            
            // deletar usuário não existente
            result = await _service.Delete(Guid.NewGuid());
            Assert.False(result);
        }
    }
}
