using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APIBia.Application.Entities
{   
    [Table("login")]
    public class LoginEntity
    {
		[Column("loginId")]
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LoginId { get; set; }

		[Column("password", TypeName = "varchar(255)")]
        public string Password { get; set; }

		[Column("userLogin", TypeName = "varchar(100)")]
        public string UserLogin { get; set; }

		[Column("name", TypeName = "varchar(255)")]
        public string Name { get; set; }

		[Column("createDate", TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }

		[Column("ativo", TypeName = "bit(1)")]        
        public bool Ativo { get; set; }

		[Column("updated", TypeName = "timestamp")]        
        public DateTime Updated { get; set; }        
    }
}
