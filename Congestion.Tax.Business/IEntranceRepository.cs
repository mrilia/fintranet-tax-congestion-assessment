
using Congestion.Tax.Business.Models.Entrance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Congestion.Tax.Business
{
    public interface IEntranceRepository
    {
        Task<IEnumerable<Entrance>> GetAllAsync();
        Task<Entrance> AddAsync(Entrance entrance);
    }
}

