using System;
using System.Collections.Generic;
using System.Text;

namespace APIBia.Application.Contracts
{
    public class LoginResponse
    {
        public int UserID { get; set; }
        public string UserLogin { get; set;}
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Active { get; set; }
        public string Password { get; set; }
    }
}
