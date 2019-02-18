using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Dbo
{
    public partial class TodoItems
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "character varying")]
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("TodoItems")]
        public Users User { get; set; }
    }
}
