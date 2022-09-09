using AutoMapper;
using SoccerKing.Api.CrossCutting.Mappings;
using System;

namespace SoccerKing.Api.Service.Test
{
    public abstract class BaseTestService
    {
        public IMapper Mapper { get; set; }

        protected BaseTestService()
        {
            Mapper = new AutoMapperFixture().GetMapper();
        }

        public class AutoMapperFixture : IDisposable
        {
            public IMapper GetMapper()
            {
                MapperConfiguration config = new(conf =>
                {
                    conf.AddProfile(new DtoToModelProfile());
                    conf.AddProfile(new EntityToDtoProfile());
                    conf.AddProfile(new ModelToEntityProfile());
                });

                return config.CreateMapper();
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);                
            }

            protected virtual void Dispose(bool disposing)
            {
                
            }
        }
    }
}
