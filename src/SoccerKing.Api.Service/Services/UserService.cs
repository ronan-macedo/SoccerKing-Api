using SoccerKing.Api.Domain.Entities;
using SoccerKing.Api.Domain.Interfaces;
using SoccerKing.Api.Domain.Interfaces.Services.User;
using SoccerKing.Api.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoccerKing.Api.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Delete(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<UserEntity> Get(Guid id)
        {
            return await _repository.SelectAsync(id);
        }

        public async Task<IEnumerable<UserEntity>> GetAll()
        {
            return await _repository.SelectAsync();
        }

        public async Task<UserEntity> Post(UserEntity user)
        {
            return await _repository.UserInsert(user);
        }

        public async Task<UserEntity> Put(UserEntity user)
        {
            return await _repository.UserUpdate(user);
        }
    }
}
