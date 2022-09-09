using Moq;
using SoccerKing.Api.Domain.Dtos.User;
using SoccerKing.Api.Domain.Interfaces.Services.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SoccerKing.Api.Service.Test.User
{
    public class ExecuteGetAll : UserTest
    {
        private IUserService _service;
        private Mock<IUserService> _serviceMock;

        [Fact(DisplayName = "Is possible execute get all method")]
        public async Task IsPossible_Execute_GetAll()
        {
            // Lista preenchida
            _serviceMock = new();
            _serviceMock.Setup(m => m.GetAll()).ReturnsAsync(listUserDto);
            _service = _serviceMock.Object;

            IEnumerable<UserDto> result = await _service.GetAll();
            Assert.NotNull(result);
            Assert.Equal(10, result.Count());

            // Lista vazia
            List<UserDto> listEmpty = new();
            _serviceMock = new();
            _serviceMock.Setup(m => m.GetAll()).ReturnsAsync(listEmpty.AsEnumerable);
            _service = _serviceMock.Object;

            IEnumerable<UserDto> resultEmpty = await _service.GetAll();
            Assert.Empty(resultEmpty);            
        }
    }
}
