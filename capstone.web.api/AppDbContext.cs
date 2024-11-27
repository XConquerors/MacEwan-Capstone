using capstone.web.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace capstone.web.api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<ToDo>()
                .HasOne(t => t.category)
                .WithMany(c => c.Todos)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ToDo>()
                .HasOne(t => t.Priority)
                .WithMany(p => p.Todos)
                .HasForeignKey(t => t.PriorityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasQueryFilter(c => !c.IsDeleted);

            modelBuilder.Entity<Priority>()
                .HasQueryFilter(p => !p.IsDeleted);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<ToDo> ToDos { get; set; }
    }
}
