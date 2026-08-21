using SportsLeague.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsLeague.Application.Contract
{
    public interface IPlayerService
    {
        Task<IEnumerable<PlayerDto>> GetAllPlayersAsync();

        // Sobrecarga 1: Obtener jugadores filtrados por ID de equipo
        Task<IEnumerable<PlayerDto>> GetAllPlayersAsync(int? teamIdFilter);

        Task<PlayerDto?> GetPlayerByIdAsync(int id);

        Task<PlayerDto> CreatePlayerAsync(PlayerDto playerDto);

        // Sobrecarga 2: Crear jugador pasando parámetros individuales
        Task<PlayerDto> CreatePlayerAsync(string fullName, string position, int teamId);

        Task<bool> UpdatePlayerAsync(int id, PlayerDto playerDto);
        Task<bool> DeletePlayerAsync(int id);
    }
}

