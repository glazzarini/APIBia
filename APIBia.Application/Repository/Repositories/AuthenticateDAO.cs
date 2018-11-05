using System;
using System.Collections.Generic;
using System.Text;
using APIBia.Application.Contracts;
using Microsoft.Extensions.Configuration;

namespace APIBia.Application.Repository.Repositories
{
    public class AuthenticateDAO
    {
        private IConfiguration _configuration;

        public AuthenticateDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthenticateRequest Find(string userID)
        {
            return new AuthenticateRequest() { UserID = "admin", AccessKey = "guest" };
        }
    }
}
