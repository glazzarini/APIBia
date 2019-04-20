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
    public class LoginController : Controller
    {
        /// <summary>
        /// Valida Usuário e Senha da Aplicação
        /// </summary>
        /// <response code="500">Falha ao Processar a solicitação</response>
        /// <param name="request"></param>
        /// <param name="loginDAO"></param>
        /// <returns>retorna dados do usuário logado</returns>
        [Authorize("Bearer")]
        [HttpGet]        
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        public IActionResult Get([FromQuery] LoginRequest request, [FromServices] LoginRepository loginDAO)
        {
            try
            {
                bool validaLogin = false; 
                if (request != null && (!string.IsNullOrEmpty(request.UserLogin) || !string.IsNullOrEmpty(request.Password)))
                {
                    var Login = loginDAO.GetAuth(request);

                    validaLogin = Login != null &&
                                  Login.UserLogin.Equals(request.UserLogin) &&
                                  Login.Password.Equals(request.Password);

                    if (validaLogin)
                    {
                        return Ok(Login);
                    }
                    else
                        return NotFound(new { Message = "Usuário não encontrado!" });
                }
                else
                    return BadRequest(new { Message = "Informe o Login e a Senha!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error. " + ex.Message);
            }
        }

        [Authorize("Bearer")]
        [HttpGet("PesMetros/{alturaPes}")]
        public object Get(double alturaPes)
        {
            return new
            {
                AlturaPes = alturaPes,
                AlturaMetros = Math.Round(alturaPes * 0.3048, 4)
            };
        }
    }
}