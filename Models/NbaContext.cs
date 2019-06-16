using Microsoft.EntityFrameworkCore;

namespace NbaApi.Models
{
    public class NbaContext : DbContext
    {
        public NbaContext(DbContextOptions<NbaContext> options)
            : base(options)
        {
        }
        
        public DbSet<NbaPlayer> NbaPlayers { get; set; }
        public DbSet<NbaClub> NbaClubs { get; set; }
    }
}
