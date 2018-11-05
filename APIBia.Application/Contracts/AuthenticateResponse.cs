using System;
using System.Collections.Generic;
using System.Text;

namespace APIBia.Application.Contracts
{
    public class AuthenticateResponse
    {
        public string CreatedDate { get; set; }
        public string ExpirationDate { get; set; }
        public bool Authenticated { get; set; }
        public string AccessToken { get; set; }
    }
}
