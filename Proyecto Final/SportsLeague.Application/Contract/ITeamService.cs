using SportsLeague.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsLeague.Application.Contract
{
    public interface ITeamService
    {
        Task<IEnumerable<TeamDto>> GetAllTeamsAsync();
        
        // Sobrecarga 1: Obtener equipos filtrando opcionalmente por ciudad
        Task<IEnumerable<TeamDto>> GetAllTeamsAsync(string? cityFilter);

        Task<TeamDto?> GetTeamByIdAsync(int id);

        Task<TeamDto> CreateTeamAsync(TeamDto teamDto);

        // Sobrecarga 2: Crear equipo pasando directamente nombre y ciudad
        Task<TeamDto> CreateTeamAsync(string name, string city);

        Task<bool> UpdateTeamAsync(int id, TeamDto teamDto);
        Task<bool> DeleteTeamAsync(int id);
    }
}

