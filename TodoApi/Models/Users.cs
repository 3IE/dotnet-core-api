using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
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
        public string Name { get; set; }

        [InverseProperty("User")]
        public ICollection<TodoItems> TodoItems { get; set; }
    }
}
