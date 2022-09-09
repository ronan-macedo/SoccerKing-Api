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
    public class UserTestData : BaseTestData, IClassFixture<BaseTestData.DbTest>
    {
        private readonly ServiceProvider _service;

        public UserTestData(DbTest dbTest)
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
            UserEntity entity = new()
            {
                Name = Faker.Name.FullName(),
                Email = Faker.Internet.Email(),
                Password = BCrypt.Net.BCrypt.HashPassword(Faker.RandomNumber.Next(99999).ToString())
            };

            // Criar novo usuário
            UserEntity userInsert = await _userRepository.UserInsertAsync(entity);
            Assert.NotNull(userInsert);
            Assert.Equal(entity.Name, userInsert.Name);
            Assert.Equal(entity.Email, userInsert.Email);
            Assert.Equal(entity.Password, userInsert.Password);
            Assert.Null(userInsert.UpdateAt);
            Assert.NotNull(userInsert.CreateAt);
            Assert.True(userInsert.Id != Guid.Empty);

            // Editar usuário
            entity.Name = Faker.Name.FullName();
            UserEntity userUpdate = await _userRepository.UserUpdateAsync(entity);
            Assert.NotNull(userUpdate);
            Assert.Equal(entity.Name, userUpdate.Name);
            Assert.Equal(entity.Email, userUpdate.Email);
            Assert.NotNull(userUpdate.UpdateAt);

            // Registro existe
            bool userExist = await _userRepository.ExistAsync(entity.Id);
            Assert.True(userExist);

            // Selecionar usuário
            UserEntity userSelect = await _userRepository.SelectAsync(entity.Id);
            Assert.NotNull(userSelect);
            Assert.Equal(entity.Name, userSelect.Name);
            Assert.Equal(entity.Email, userSelect.Email);

            // Selecionar todos os registros
            IEnumerable<UserEntity> allUsers = await _userRepository.SelectAsync();
            Assert.NotNull(allUsers);
            Assert.NotEmpty(allUsers);

            // Deletar usuário
            bool userDelete = await _userRepository.DeleteAsync(entity.Id);
            Assert.True(userDelete);
        }
    }
}
