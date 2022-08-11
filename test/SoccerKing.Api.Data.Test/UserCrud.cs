using Microsoft.Extensions.DependencyInjection;
using SoccerKing.Api.Data.Context;
using SoccerKing.Api.Data.Repository;
using SoccerKing.Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SoccerKing.Api.Data.Test
{
    public class UserCrud : BaseTestData, IClassFixture<BaseTestData.DbTest>
    {
        private readonly ServiceProvider _service;

        public UserCrud(DbTest dbTest)
        {
            _service = dbTest.ServiceProvider;
        }

        [Fact(DisplayName = "User Crud")]
        [Trait("User", "UserCrud")]
        public async Task IsPossible_User_Crud()
        {
            using MyDbContext context = _service.GetService<MyDbContext>();
            UserRepository _userRepository = new(context);

            // Inicializar a entidade
            UserEntity _entity = new()
            {
                Name = Faker.Name.FullName(),
                Email = Faker.Internet.Email(),
                Password = BCrypt.Net.BCrypt.HashPassword(Faker.RandomNumber.Next(99999).ToString())
            };

            // Criar novo usuário
            UserEntity _userInsert = await _userRepository.UserInsertAsync(_entity);
            Assert.NotNull(_userInsert);
            Assert.Equal(_entity.Name, _userInsert.Name);
            Assert.Equal(_entity.Email, _userInsert.Email);
            Assert.Equal(_entity.Password, _userInsert.Password);
            Assert.Null(_userInsert.UpdateAt);
            Assert.NotNull(_userInsert.CreateAt);
            Assert.True(_userInsert.Id != Guid.Empty);

            // Editar usuário
            _entity.Name = Faker.Name.FullName();
            UserEntity _userUpdate = await _userRepository.UserUpdateAsync(_entity);
            Assert.NotNull(_userUpdate);
            Assert.Equal(_entity.Name, _userUpdate.Name);
            Assert.Equal(_entity.Email, _userUpdate.Email);
            Assert.NotNull(_userUpdate.UpdateAt);

            // Registro existe
            bool _userExist = await _userRepository.ExistAsync(_entity.Id);
            Assert.True(_userExist);

            // Selecionar usuário
            UserEntity _userSelect = await _userRepository.SelectAsync(_entity.Id);
            Assert.NotNull(_userSelect);
            Assert.Equal(_entity.Name, _userSelect.Name);
            Assert.Equal(_entity.Email, _userSelect.Email);

            // Selecionar todos os registros
            IEnumerable<UserEntity> _allUsers = await _userRepository.SelectAsync();
            Assert.NotNull(_allUsers);
            Assert.NotEmpty(_allUsers);

            // Deletar usuário
            bool _userDelete = await _userRepository.DeleteAsync(_entity.Id);
            Assert.True(_userDelete);
        }
    }
}
