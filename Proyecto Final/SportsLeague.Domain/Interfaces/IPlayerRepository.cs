using System.Collections.Generic;
using System.Threading.Tasks;
using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces
{
    public interface IPlayerRepository : IBaseRepository<Player>
    {
        Task<IEnumerable<Player>> GetPlayersByTeamIdAsync(int teamId);
        Task<IEnumerable<Player>> GetAllPlayersWithTeamAsync();
        Task<Player?> GetPlayerWithTeamAsync(int id);
        Task<IEnumerable<Player>> GetPlayersByPositionAsync(string position);
    }
}

