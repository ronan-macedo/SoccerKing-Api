using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SoccerKing.Api.Domain.Dtos;
using SoccerKing.Api.Domain.Entities;
using SoccerKing.Api.Domain.Interfaces.Services.User;
using SoccerKing.Api.Domain.Repository;
using SoccerKing.Api.Domain.Security;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace SoccerKing.Api.Service.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _repository;
        private readonly SigningConfiguration _signinConfiguration;
        private readonly TokenConfiguration _tokenConfiguration;

        public LoginService(ILoginRepository repository,
                            SigningConfiguration signinConfiguration,
                            TokenConfiguration tokenConfiguration)
        {
            _repository = repository;
            _signinConfiguration = signinConfiguration;
            _tokenConfiguration = tokenConfiguration;
        }

        public async Task<object> FindByLogin(LoginDto login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
                return null;

            UserEntity baseUser = await _repository.FindByLogin(login.Email, login.Password);

            if (baseUser == null)
                return new
                {
                    authenticated = false,
                    message = "Falha ao autenticar"
                };

            ClaimsIdentity identity = new(
                new GenericIdentity(login.Email),
                new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Id do token
                    new Claim(JwtRegisteredClaimNames.UniqueName, login.Email)
                }
            );

            DateTime createDate = DateTime.UtcNow;
            DateTime expirationDate = createDate + TimeSpan.FromSeconds(_tokenConfiguration.Seconds);
            JwtSecurityTokenHandler handler = new();
            string token = CreateToken(identity, createDate, expirationDate, handler);

            return SucessObject(createDate, expirationDate, token, login);
        }

        private string CreateToken(ClaimsIdentity identity,
                                   DateTime createDate,
                                   DateTime expirationDate,
                                   JwtSecurityTokenHandler handler)
        {
            SecurityToken securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfiguration.Issuer,
                Audience = _tokenConfiguration.Audience,
                SigningCredentials = _signinConfiguration.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate
            });
            string token = handler.WriteToken(securityToken);

            return token;
        }

        private static object SucessObject(DateTime createDate, DateTime expirationDate, string token, LoginDto login)
        {
            return new
            {
                authenticated = true,
                created = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                accessToken = token,
                userName = login.Email,
                message = "Usuário logado com sucesso"
            };
        }
    }
}
