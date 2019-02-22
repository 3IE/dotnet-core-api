using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TodoItems> TodoItems { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItems>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"TodoItems_id_seq\"'::regclass)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TodoItems)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TodoItems_UserId_fkey");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"User_id_seq\"'::regclass)");
            });

            modelBuilder.HasSequence<int>("TodoItems_id_seq");

            modelBuilder.HasSequence<int>("User_id_seq");
        }
    }
}
