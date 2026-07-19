using SportsLeague.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsLeague.Application.Contract
{
    public interface IPlayerService
    {
        Task<IEnumerable<PlayerDto>> GetAllPlayersAsync();
        Task<PlayerDto?> GetPlayerByIdAsync(int id);
        Task<PlayerDto> CreatePlayerAsync(PlayerDto playerDto);
        Task<bool> UpdatePlayerAsync(int id, PlayerDto playerDto);
        Task<bool> DeletePlayerAsync(int id);
    }
}
