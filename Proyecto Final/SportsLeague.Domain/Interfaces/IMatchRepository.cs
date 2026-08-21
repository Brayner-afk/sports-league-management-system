using SportsLeague.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsLeague.Domain.Interfaces
{
    public interface IMatchRepository : IBaseRepository<Match>
    {
        Task<IEnumerable<Match>> GetAllMatchesWithTeamsAsync();
        Task<Match?> GetMatchWithTeamsByIdAsync(int id);
        Task<IEnumerable<Match>> GetMatchesByTeamIdAsync(int teamId);
        Task<IEnumerable<Match>> GetMatchesByStatusAsync(string status);
    }
}