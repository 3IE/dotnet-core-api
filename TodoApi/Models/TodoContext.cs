using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    // Derive de la classe Microsoft.EntityFrameworkCore.DbContext
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}