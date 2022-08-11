using AutoMapper;
using SoccerKing.Api.Domain.Dtos.User;
using SoccerKing.Api.Domain.Models;

namespace SoccerKing.Api.CrossCutting.Mappings
{
    public class DtoToModelProfile : Profile
    {
        public DtoToModelProfile()
        {
            CreateMap<UserModel, UserDto>()
                .ReverseMap();
            CreateMap<UserModel, UserDtoCreate>()
                .ReverseMap();
            CreateMap<UserModel, UserDtoUpdate>()
                .ReverseMap();
        }
    }
}
