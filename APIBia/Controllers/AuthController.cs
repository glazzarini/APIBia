using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using APIBia.Application.Configuration;
using APIBia.Application.Contracts;
using APIBia.Application.Repository.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace APIBia.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly AuthenticateRepository _authenticateRepository;
        private readonly SigningConfigurations _signingConfigurations;
        private readonly TokenConfiguration _tokenConfiguration;

        public AuthController(AuthenticateRepository authenticateRepository,
                              SigningConfigurations signingConfigurations,
                              TokenConfiguration tokenConfiguration)
        {
            _authenticateRepository = authenticateRepository;
            _signingConfigurations = signingConfigurations;
            _tokenConfiguration = tokenConfiguration;
        }        

        /// <summary>
        /// Obtem Token de Autenticação
        /// </summary>
        /// <response code="200">Token e Tempo de Expiração</response>
        /// <response code="401">Permissão de acesso negada</response>
        /// <response code="400">Formato inválido do(s) paramêtro(s) de entrada</response>
        /// <response code="500">Falha ao Processar a solicitação</response>
        /// <param name="usuario">Dados de Login e Senha</param>
        /// <param name="usersDAO">Objeto DAO para Validação de Login e Senha</param>
        /// <param name="signingConfigurations">Objeto de configuração de acesso</param>
        /// <param name="tokenConfigurations">Objeto com Token</param>
        /// <returns>Token Autenticado</returns>
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(AuthenticateResponse), (int)HttpStatusCode.OK)]
        public IActionResult Post([FromBody] AuthenticateRequest request)
        {
            try
            {
                AuthenticateResponse response = new AuthenticateResponse();

                bool credenciaisValidas = false;
                if (request != null && !String.IsNullOrWhiteSpace(request.UserLogin))
                {
                    var usuarioAuth = _authenticateRepository.GetAuth(request);
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

                    return Ok(response);
                }
                else
                    return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error. " + ex.Message);
            }
        }
    }
}