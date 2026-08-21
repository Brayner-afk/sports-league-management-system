using SportsLeague.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsLeague.WebAssembly.Services
{
    public interface ISportsLeagueApiService
    {
        // Equipos
        Task<IEnumerable<TeamDto>> GetTeamsAsync(string? cityFilter = null);
        Task<TeamDto?> GetTeamByIdAsync(int id);
        Task<TeamDto?> CreateTeamAsync(TeamDto teamDto);
        Task<bool> UpdateTeamAsync(int id, TeamDto teamDto);
        Task<bool> DeleteTeamAsync(int id);

        // Jugadores
        Task<IEnumerable<PlayerDto>> GetPlayersAsync(int? teamIdFilter = null);
        Task<PlayerDto?> GetPlayerByIdAsync(int id);
        Task<PlayerDto?> CreatePlayerAsync(PlayerDto playerDto);
        Task<bool> UpdatePlayerAsync(int id, PlayerDto playerDto);
        Task<bool> DeletePlayerAsync(int id);

        // Partidos y Calendario
        Task<IEnumerable<MatchDto>> GetMatchesAsync(string? statusFilter = null, int? teamIdFilter = null);
        Task<MatchDto?> GetMatchByIdAsync(int id);
        Task<MatchDto?> CreateMatchAsync(MatchDto matchDto);
        Task<bool> UpdateMatchAsync(int id, MatchDto matchDto);
        Task<bool> DeleteMatchAsync(int id);

        // Estado del servicio
        Task<bool> CheckApiHealthAsync();
        string GetApiBaseUrl();
    }
}