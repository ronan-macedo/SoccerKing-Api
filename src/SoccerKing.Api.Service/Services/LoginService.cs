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

        public LoginService(ILoginRepository repository,
                            SigningConfiguration signinConfiguration)
        {
            _repository = repository;
            _signinConfiguration = signinConfiguration;            
        }

        /// <summary>
        /// Função responsável por realizar o processo de autenticação e 
        /// criação do token
        /// </summary>
        /// <param name="login">Recebe e-mail e senha</param>
        /// <returns>Retorna um objeto contendo o token</returns>
        public async Task<object> FindByLogin(LoginDto login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
                return FailObject();

            UserEntity baseUser = await _repository.FindByLogin(login.Email, login.Password);

            if (baseUser == null)
                return FailObject();

            /**
             * As claim são pedaços de informações reivindicadas por um sujeito
             * por exemplo: o Id do token ou o nome do usuário
             */
            ClaimsIdentity identity = new(
                new GenericIdentity(login.Email),
                new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Id do token
                    new Claim(JwtRegisteredClaimNames.UniqueName, login.Email)                    
                }
            );

            DateTime createDate = DateTime.UtcNow;
            DateTime expirationDate = createDate + TimeSpan.FromSeconds(int.Parse(Environment.GetEnvironmentVariable("Seconds")));
            JwtSecurityTokenHandler handler = new();
            string token = CreateToken(identity, createDate, expirationDate, handler);

            return SucessObject(createDate, expirationDate, token, baseUser);
        }

        /// <summary>
        /// Método responsável por gerar o token
        /// </summary>
        /// <param name="identity">Claims do Jwt</param>
        /// <param name="createDate">Data de criação do token</param>
        /// <param name="expirationDate">Data de expiração do token</param>
        /// <param name="handler">Objeto reponsável para criar o token</param>
        /// <returns>A string do token</returns>
        private string CreateToken(ClaimsIdentity identity, DateTime createDate, DateTime expirationDate, JwtSecurityTokenHandler handler)
        {
            SecurityToken securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = Environment.GetEnvironmentVariable("Issuer"),
                Audience = Environment.GetEnvironmentVariable("Audience"),
                SigningCredentials = _signinConfiguration.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate
            });
            string token = handler.WriteToken(securityToken);

            return token;
        }

        #region Objetos de retorno
        /// <summary>
        /// Monta o objeto de retorno após a criação do token
        /// </summary>
        /// <param name="createDate">Data de criação do token</param>
        /// <param name="expirationDate">Data de expiração do token</param>
        /// <param name="token">String do token</param>
        /// <param name="user">Objeto contendo e-mail e senha</param>
        /// <returns>Objeto que irá se retornado no método FindByLogin</returns>
        private static object SucessObject(DateTime createDate, DateTime expirationDate, string token, UserEntity user)
        {
            return new
            {
                authenticated = true,
                created = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                accessToken = token,
                userName = user.Name,
                userEmail = user.Email,
                message = "Usuário logado com sucesso"
            };
        }

        /// <summary>
        /// Monta o objeto de retorno se houver falha
        /// </summary>
        /// <returns>Objeto falha ao autenticar</returns>
        private static object FailObject()
        {
            return new
            {
                authenticated = false,
                message = "Falha ao autenticar"
            };
        }
        #endregion
    }
}
