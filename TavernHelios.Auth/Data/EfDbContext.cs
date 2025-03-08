using Microsoft.EntityFrameworkCore;
using TavernHelios.Auth.Models;

namespace TavernHelios.Auth.Data
{
    public class EfDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
        }
    }
}
