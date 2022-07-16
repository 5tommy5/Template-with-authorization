using Microsoft.EntityFrameworkCore;
using Users.Domain;
using Users.Domain.Entities;

namespace Template.Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RolesSeed());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<User> Roles { get; set; }

    }
}
