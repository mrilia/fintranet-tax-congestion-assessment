using Congestion.Tax.Business.Models.Entrance;
using Microsoft.EntityFrameworkCore;

namespace Congestion.Tax.Persistence.EF
{
    public class TaxDbContext : DbContext
    {
        public TaxDbContext(DbContextOptions<TaxDbContext> options) : base(options)
        {
        }

        public DbSet<Entrance> Entrances { get; set; }

        public Entrance AddInterance(Entrance entrance)
        {
            Entrances.Add(entrance); 
            return entrance;
        }

        public IQueryable<Entrance> GetAllEntrances()
        {
            return Entrances.AsQueryable();
        }
    }
}