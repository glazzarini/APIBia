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
    public class LoginRepository
    {
        private IConfiguration _configuration;

        public LoginRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private MySqlConnection GetConnection()
        {   
            return new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public LoginEntity GetAuth(LoginRequest request)
        {
            using (MySqlConnection conexao = new MySqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                return conexao.QueryFirstOrDefault<LoginEntity>(
                    @"SELECT * FROM LOGIN 
                      WHERE USER_LOGIN = @UserLogin AND PASSWORD = @Password", new { request.UserLogin, request.Password });
            }
        }
    }
}
