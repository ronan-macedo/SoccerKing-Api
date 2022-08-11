using AutoMapper;
using SoccerKing.Api.Domain.Entities;
using SoccerKing.Api.Domain.Models;

namespace SoccerKing.Api.CrossCutting.Mappings
{
    public class ModelToEntityProfile: Profile
    {
        public ModelToEntityProfile()
        {
            CreateMap<UserEntity, UserModel>()
                .ReverseMap();
        }
    }
}
