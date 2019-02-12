using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIBia.Application.Entities
{   
    public class Login
    {
        [Required]
        public int ID_LOGIN { get; set; }
        [Required]
        public string PASSWORD { get; set; }
        [Required]
        public string USER_LOGIN { get; set; }
        [Required]
        public string NAME { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public bool ACTIVE { get; set; }
    }
}
