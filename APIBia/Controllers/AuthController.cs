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
        public IActionResult Post(
            [FromBody] AuthenticateRequest usuario,
            [FromServices]AuthenticateDAO usersDAO,
            [FromServices]SigningConfigurations signingConfigurations,
            [FromServices]TokenConfiguration tokenConfigurations)
        {
            try
            {
                AuthenticateResponse response = new AuthenticateResponse();

                bool credenciaisValidas = false;
                if (usuario != null && !String.IsNullOrWhiteSpace(usuario.UserID))
                {
                    var usuarioBase = usersDAO.Find(usuario.UserID);
                    credenciaisValidas = (usuarioBase != null &&
                        usuario.UserID == usuarioBase.UserID &&
                        usuario.AccessKey == usuarioBase.AccessKey);
                }

                if (credenciaisValidas)
                {
                    ClaimsIdentity identity = new ClaimsIdentity(
                        new GenericIdentity(usuario.UserID, "Login"),
                        new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserID)
                        }
                    );

                    DateTime dataCriacao = DateTime.Now;
                    DateTime dataExpiracao = dataCriacao +
                        TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                    {
                        Issuer = tokenConfigurations.Issuer,
                        Audience = tokenConfigurations.Audience,
                        SigningCredentials = signingConfigurations.SigningCredentials,
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