using SoccerKing.Api.Domain.Dtos.User;
using SoccerKing.Api.Domain.Entities;
using SoccerKing.Api.Domain.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace SoccerKing.Api.Service.Test.AutoMapper
{
    public class UserMapper : BaseTestService
    {
        [Fact(DisplayName = "Is possible mapping models")]
        public void IsPossible_Mapping_Models()
        {
            // model inicial
            UserModel model = new()
            {
                Id = Guid.NewGuid(),
                Name = Faker.Name.FullName(),
                Email = Faker.Internet.Email(),
                Password = BCrypt.Net.BCrypt.HashPassword(Faker.RandomNumber.Next().ToString()),
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };

            // lista users para retorno de listas
            List<UserEntity> listEntity = new();
            for (int i = 0; i < 5; i++)
            {
                UserEntity user = new()
                {
                    Id = Guid.NewGuid(),
                    Name = Faker.Name.FullName(),
                    Email = Faker.Internet.Email(),
                    Password = BCrypt.Net.BCrypt.HashPassword(Faker.RandomNumber.Next().ToString()),
                    CreateAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow
                };

                listEntity.Add(user);
            }

            // Model para Entity
            UserEntity entity = Mapper.Map<UserEntity>(model);
            Assert.Equal(model.Id, entity.Id);
            Assert.Equal(model.Name, entity.Name);
            Assert.Equal(model.Email, entity.Email);
            Assert.Equal(model.Password, entity.Password);
            Assert.Equal(model.CreateAt, entity.CreateAt);
            Assert.Equal(model.UpdateAt, entity.UpdateAt);

            // Entity para Dto
            UserDto dto = Mapper.Map<UserDto>(entity);
            Assert.Equal(entity.Id, dto.Id);
            Assert.Equal(entity.Name, dto.Name);
            Assert.Equal(entity.Email, dto.Email);
            Assert.Equal(entity.CreateAt, dto.CreateAt);

            // ListEntity para ListDto
            List<UserDto> listDto = Mapper.Map<List<UserDto>>(listEntity);
            Assert.Equal(listEntity.Count, listDto.Count);
            for (int i = 0; i < listDto.Count; i++)
            {
                Assert.Equal(listEntity[i].Id, listDto[i].Id);
                Assert.Equal(listEntity[i].Name, listDto[i].Name);
                Assert.Equal(listEntity[i].Email, listDto[i].Email);
                Assert.Equal(listEntity[i].CreateAt, listDto[i].CreateAt);
            }

            // Entity para DtoCreateResult
            UserDtoCreateResult dtoCreateResult = Mapper.Map<UserDtoCreateResult>(entity);
            Assert.Equal(entity.Id, dtoCreateResult.Id);
            Assert.Equal(entity.Name, dtoCreateResult.Name);
            Assert.Equal(entity.Email, dtoCreateResult.Email);
            Assert.Equal(entity.CreateAt, dtoCreateResult.CreateAt);

            // Entity para DtoUpdateResult
            UserDtoUpdateResult dtoUpdateResult = Mapper.Map<UserDtoUpdateResult>(entity);
            Assert.Equal(entity.Id, dtoUpdateResult.Id);
            Assert.Equal(entity.Name, dtoUpdateResult.Name);
            Assert.Equal(entity.Email, dtoUpdateResult.Email);            
            Assert.Equal(entity.UpdateAt, dtoUpdateResult.UpdateAt);

            // Dto para Model
            UserModel userModel = Mapper.Map<UserModel>(dto);
            Assert.Equal(dto.Id, userModel.Id);
            Assert.Equal(dto.Name, userModel.Name);
            Assert.Equal(dto.Email, userModel.Email);            
            Assert.Equal(dto.CreateAt, userModel.CreateAt);

            // Model para DtoCreate
            UserDtoCreate dtoCreate = Mapper.Map<UserDtoCreate>(model);            
            Assert.Equal(model.Name, dtoCreate.Name);
            Assert.Equal(model.Email, dtoCreate.Email);
            Assert.Equal(model.Password, dtoCreate.Password);

            // Model para DtoUpdate
            UserDtoUpdate userDto = Mapper.Map<UserDtoUpdate>(model);
            Assert.Equal(model.Id, userDto.Id);
            Assert.Equal(model.Name, userDto.Name);
            Assert.Equal(model.Email, userDto.Email);
            Assert.Equal(model.Password, entity.Password);            
        }
    }
}
