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
    public class LoginDAO
    {
        private IConfiguration _configuration;

        public LoginDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public LoginResponse Find2(LoginRequest request)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM LOGIN;");

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                    }
                }
            }

            return new LoginResponse();
        }

        public Login Find(LoginRequest request)
        {
            using (MySqlConnection conexao = new MySqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
                return conexao.QueryFirstOrDefault<Login>(
                    @"SELECT * FROM LOGIN 
                      WHERE USER_LOGIN = @UserLogin AND PASSWORD = @Password", new { request.UserLogin, request.Password });
            }
        }
    }
}
