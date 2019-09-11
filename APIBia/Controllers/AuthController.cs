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
using APIBia.Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace APIBia.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        private readonly AuthenticateRepository _authenticateRepository;
        private readonly SigningConfigurations _signingConfigurations;
        private readonly TokenConfiguration _tokenConfiguration;

        public AuthController(AuthService authService,
                              AuthenticateRepository authenticateRepository,
                              SigningConfigurations signingConfigurations,
                              TokenConfiguration tokenConfiguration)
        {
            _authService = authService;
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
        public async Task<IActionResult> Post([FromBody] AuthenticateRequest request)
        {
            var result = await _authService.GetAuth(request);
            return Ok(result);
        }
    }
}