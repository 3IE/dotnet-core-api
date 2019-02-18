using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Dbo
{
    public partial class Users
    {
        public Users()
        {
            TodoItems = new HashSet<TodoItems>();
        }

        public int Id { get; set; }
        [Required]
        [Column(TypeName = "character varying")]
        public string Username { get; set; }
        [Required]
        [Column(TypeName = "character varying")]
        public string Password { get; set; }
        [Column(TypeName = "character varying")]
        public string Token { get; set; }

        [InverseProperty("User")]
        public ICollection<TodoItems> TodoItems { get; set; }
    }
}
