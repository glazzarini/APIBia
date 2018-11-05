using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIBia.Application.Contracts
{
    public class LoginRequest
    {
        [Required]
        public string UserLogin { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
