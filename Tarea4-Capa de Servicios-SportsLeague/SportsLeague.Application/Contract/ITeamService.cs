using SportsLeague.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsLeague.Application.Contract
{
    public interface ITeamService
    {
        Task<IEnumerable<TeamDto>> GetAllTeamsAsync();
        Task<TeamDto?> GetTeamByIdAsync(int id);
        Task<TeamDto> CreateTeamAsync(TeamDto teamDto);
        Task<bool> UpdateTeamAsync(int id, TeamDto teamDto);
        Task<bool> DeleteTeamAsync(int id);
    }
}
