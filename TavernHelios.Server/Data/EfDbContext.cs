using Microsoft.EntityFrameworkCore;
using TavernHelios.Server.Data.Models;

namespace TavernHelios.Server.Data
{
    public class EfDbContext : DbContext
    {
        public DbSet<HeliosUser> Users { get; set; }

        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
        }
    }
}
