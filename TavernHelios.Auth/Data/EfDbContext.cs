using Microsoft.EntityFrameworkCore;
using TavernHelios.Auth.Data.Models;

namespace TavernHelios.Auth.Data
{
    public class EfDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UsersRoles { get; set; }
        public DbSet<Role> Roles { get; set; }

        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>().HasKey(x => new { x.UserId, x.RoleId });
            builder.Entity<User>()
                .HasMany(x => x.UserRoles)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
        }
    }
}
