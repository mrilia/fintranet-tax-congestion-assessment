using Congestion.Tax.Business;
using Congestion.Tax.Business.Models.Entrance;
using Microsoft.EntityFrameworkCore;

namespace Congestion.Tax.Persistence.EF
{
    public class EntranceRepository : IEntranceRepository
    {
        private readonly TaxDbContext _dbContext;

        public EntranceRepository(TaxDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Entrance>> GetAllAsync()
        {
            return await _dbContext.Entrances.ToListAsync();
        }

        public async Task<Entrance> AddAsync(Entrance entrance)
        {
            _dbContext.Entrances.Add(entrance);
            await _dbContext.SaveChangesAsync();
            return entrance;
        }
    }
}
