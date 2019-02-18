using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TodoApi.Dbo
{
    public partial class DataContext : DbContext
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

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

            modelBuilder.HasSequence("TodoItems_id_seq");

            modelBuilder.HasSequence("User_id_seq");
        }
    }
}
