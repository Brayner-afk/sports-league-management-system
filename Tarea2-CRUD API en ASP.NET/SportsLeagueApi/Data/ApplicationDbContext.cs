using Microsoft.EntityFrameworkCore;
using SportsLeagueApi.Models;

namespace SportsLeagueApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Definimos las tablas
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
    }
}
