using SoccerKing.Api.Domain.Dtos.User;
using System;
using System.Collections.Generic;

namespace SoccerKing.Api.Service.Test.User
{
    public class UserTest
    {
        public static Guid UserId { get; set; }
        public static string UserName { get; set; }
        public static string UserEmail { get; set; }
        public static string UserPassword { get; set; }
        public static string UserNameUpdate { get; set; }
        public static string UserEmailUpdate { get; set; }
        public static string UserPasswordUpdate { get; set; }

        public List<UserDto> listUserDto = new();
        public UserDto userDto;
        public UserDtoCreate userDtoCreate;
        public UserDtoCreateResult userDtoCreateResult;
        public UserDtoUpdate userDtoUpdate;
        public UserDtoUpdateResult userDtoUpdateResult;

        public UserTest()
        {
            UserId = Guid.NewGuid();
            UserName = Faker.Name.FullName();
            UserEmail = Faker.Internet.Email();
            UserPassword = BCrypt.Net.BCrypt.HashPassword(Faker.RandomNumber.Next().ToString());
            UserNameUpdate = Faker.Name.FullName();
            UserEmailUpdate = Faker.Internet.Email();
            UserPasswordUpdate = BCrypt.Net.BCrypt.HashPassword(Faker.RandomNumber.Next().ToString());

            for (int i = 0; i < 10; i++)
            {
                UserDto user = new()
                {
                    Id = Guid.NewGuid(),
                    Name = Faker.Name.FullName(),
                    Email = Faker.Internet.Email(),
                    CreateAt = DateTime.UtcNow
                };

                listUserDto.Add(user);
            }

            userDto = new()
            {
                Id = UserId,
                Name = UserName,
                Email = UserEmail,
                CreateAt = DateTime.UtcNow
            };

            userDtoCreate = new()
            {
                Name = UserName,
                Email = UserEmail,
                Password = UserPassword
            };

            userDtoCreateResult = new()
            {
                Id = UserId,
                Name = UserName,
                Email = UserEmail,
                CreateAt = DateTime.UtcNow
            };

            userDtoUpdate = new()
            {
                Id = UserId,
                Name = UserNameUpdate,
                Email = UserEmailUpdate,
                Password = UserPasswordUpdate
            };

            userDtoUpdateResult = new()
            {
                Id = UserId,
                Name = UserNameUpdate,
                Email = UserEmailUpdate,
                UpdateAt = DateTime.UtcNow
            };
        }
    }
}
