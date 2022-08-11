using AutoMapper;
using SoccerKing.Api.Domain.Dtos.User;
using SoccerKing.Api.Domain.Entities;
using SoccerKing.Api.Domain.Interfaces;
using SoccerKing.Api.Domain.Interfaces.Services.User;
using SoccerKing.Api.Domain.Models;
using SoccerKing.Api.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoccerKing.Api.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<UserDto> Get(Guid id)
        {
            UserEntity entity = await _repository.SelectAsync(id);
            return _mapper.Map<UserDto>(entity) ?? new UserDto();
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            IEnumerable<UserEntity> listEntity = await _repository.SelectAsync();
            return _mapper.Map<IEnumerable<UserDto>>(listEntity);
        }

        public async Task<UserDtoCreateResult> Post(UserDtoCreate user)
        {
            UserModel model = _mapper.Map<UserModel>(user);
            model.UpdateAt = null;
            UserEntity entity = _mapper.Map<UserEntity>(model);
            UserEntity result = await _repository.UserInsertAsync(entity);
            return _mapper.Map<UserDtoCreateResult>(result);
        }

        public async Task<UserDtoUpdateResult> Put(UserDtoUpdate user)
        {
            UserModel model = _mapper.Map<UserModel>(user);
            UserEntity validationEntity = await _repository.SelectAsync(model.Id);            

            if (string.IsNullOrWhiteSpace(model.Email))
                model.Email = validationEntity.Email;

            if (string.IsNullOrWhiteSpace(model.Password))
                model.Password = validationEntity.Password;

            if (!model.Password.Equals(validationEntity.Password))
                model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            UserEntity entity = _mapper.Map<UserEntity>(model);
            UserEntity result = await _repository.UserUpdateAsync(entity);
            return _mapper.Map<UserDtoUpdateResult>(result);
        }
    }
}
