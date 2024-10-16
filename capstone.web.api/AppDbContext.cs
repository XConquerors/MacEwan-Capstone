using capstone.web.api;
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
            modelBuilder.Entity<Category>()
                .HasQueryFilter(c => !c.IsDeleted);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Priority> Priorities { get; set; }

    }
}
