using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIBia.Application.Contracts
{
    public class AuthenticateRequest
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        public string AccessKey { get; set; }
    }
}
