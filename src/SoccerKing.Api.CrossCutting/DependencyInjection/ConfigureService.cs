using Microsoft.Extensions.DependencyInjection;
using SoccerKing.Api.Domain.Interfaces.Services.User;
using SoccerKing.Api.Service.Services;

namespace SoccerKing.Api.CrossCutting.DependencyInjection
{
    public static class ConfigureService
    {
        public static void ConfigureDependencesService(IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IUserService, UserService>();
        }
    }
}
