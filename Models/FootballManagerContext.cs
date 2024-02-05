using Microsoft.EntityFrameworkCore;

namespace FootballManager.Models
{
    public class FootballManagerContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<FootballAssociation> FootballAssociations { get; set; }

        public FootballManagerContext(DbContextOptions options) : base(options) { }
    }
}
