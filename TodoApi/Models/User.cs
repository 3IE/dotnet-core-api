using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
