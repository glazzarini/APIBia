using APIBia.Application.Contracts;
using APIBia.Application.Entities;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Dapper;

namespace APIBia.Application.Repository.Repositories
{
    public class AuthenticateRepository
    {
        private IConfiguration _configuration;

        public AuthenticateRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public LoginEntity GetAuth(AuthenticateRequest request)
        {
            using (MySqlConnection conexao = new MySqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                return conexao.QueryFirstOrDefault<LoginEntity>(
                    @"SELECT * FROM login 
                      WHERE userLogin = @UserLogin AND password = @Password;", new { request.UserLogin, request.Password });
            }
        }
    }
}
