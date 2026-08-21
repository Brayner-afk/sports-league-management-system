using System.Collections.Generic;
using System.Threading.Tasks;
using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces
{
    public interface ITeamRepository : IBaseRepository<Team>
    {
        Task<Team?> GetTeamWithPlayersAsync(int id);
        Task<IEnumerable<Team>> GetAllTeamsWithPlayersAsync();
        Task<IEnumerable<Team>> GetTeamsByCityAsync(string city);
    }
}

