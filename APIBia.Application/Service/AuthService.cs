using APIBia.Application.Configuration;
using APIBia.Application.Contracts;
using APIBia.Application.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace APIBia.Application.Service
{
    public class AuthService
    {
        private readonly AuthenticateRepository _authenticateRepository;
        private readonly SigningConfigurations _signingConfigurations;
        private readonly TokenConfiguration _tokenConfiguration;

        public AuthService(AuthenticateRepository authenticateRepository,
                           SigningConfigurations signingConfigurations,
                           TokenConfiguration tokenConfiguration)
        {
            _authenticateRepository = authenticateRepository;
            _signingConfigurations = signingConfigurations;
            _tokenConfiguration = tokenConfiguration;
        }

        public async Task<AuthenticateResponse> GetAuth(AuthenticateRequest request)
        {
            try
            {
                AuthenticateResponse response = new AuthenticateResponse();

                bool credenciaisValidas = false;
                if (request != null && !String.IsNullOrWhiteSpace(request.UserLogin))
                {
                    var usuarioAuth = await _authenticateRepository.GetAuth(request);
                    credenciaisValidas = (usuarioAuth != null &&
                        request.UserLogin == usuarioAuth.UserLogin);
                }

                if (credenciaisValidas)
                {
                    ClaimsIdentity identity = new ClaimsIdentity(
                        new GenericIdentity(request.UserLogin, "Login"),
                        new[] {
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                            new Claim(JwtRegisteredClaimNames.UniqueName, request.UserLogin)
                        }
                    );

                    DateTime dataCriacao = DateTime.Now;
                    DateTime dataExpiracao = dataCriacao +
                        TimeSpan.FromSeconds(_tokenConfiguration.Seconds);

                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                    {
                        Issuer = _tokenConfiguration.Issuer,
                        Audience = _tokenConfiguration.Audience,
                        SigningCredentials = _signingConfigurations.SigningCredentials,
                        Subject = identity,
                        NotBefore = dataCriacao,
                        Expires = dataExpiracao
                    });

                    var token = handler.WriteToken(securityToken);

                    response = new AuthenticateResponse()
                    {
                        Authenticated = true,
                        CreatedDate = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                        ExpirationDate = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                        AccessToken = token
                    };
                }

                return response;
            }
            catch (Exception)
            {
                return new AuthenticateResponse();
            }
        }
    }
}
