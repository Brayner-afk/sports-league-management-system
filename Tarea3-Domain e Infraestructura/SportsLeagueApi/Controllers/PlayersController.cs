using Microsoft.AspNetCore.Mvc;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces;
using SportsLeague.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsLeagueApi.Controllers {
    /// <summary>
    /// Gestión de Jugadores de la Liga
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;

        public PlayersController(IPlayerRepository playerRepository, ITeamRepository teamRepository) {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
        }

        /// <summary>
        /// Obtener lista de todos los jugadores registrados
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDto>>> GetPlayers() {
            var players = await _playerRepository.GetAllAsync();
            return Ok(players.Select(p => new PlayerDto { Id = p.Id, FullName = p.FullName, Position = p.Position, TeamId = p.TeamId }));
        }

        /// <summary>
        /// Buscar un jugador específico por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDto>> GetPlayer(int id) {
            var player = await _playerRepository.GetByIdAsync(id);
            if (player == null) return NotFound();
            return Ok(new PlayerDto { Id = player.Id, FullName = player.FullName, Position = player.Position, TeamId = player.TeamId });
        }

        /// <summary>
        /// Agregar un nuevo jugador y asignarlo a un equipo
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PlayerDto>> PostPlayer(PlayerDto playerDto) {
            if (!await _teamRepository.ExistsAsync(playerDto.TeamId)) return BadRequest("El TeamId no existe.");
            var player = new Player { FullName = playerDto.FullName, Position = playerDto.Position, TeamId = playerDto.TeamId };
            await _playerRepository.AddAsync(player);
            playerDto.Id = player.Id;
            return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, playerDto);
        }

        /// <summary>
        /// Actualizar los datos de un jugador existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, PlayerDto playerDto) {
            if (id != playerDto.Id) return BadRequest();
            var player = await _playerRepository.GetByIdAsync(id);
            if (player == null) return NotFound();
            if (!await _teamRepository.ExistsAsync(playerDto.TeamId)) return BadRequest("El TeamId no existe.");
            
            player.FullName = playerDto.FullName;
            player.Position = playerDto.Position;
            player.TeamId = playerDto.TeamId;
            await _playerRepository.UpdateAsync(player);
            return NoContent();
        }

        /// <summary>
        /// Eliminar un jugador del sistema
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id) {
            var player = await _playerRepository.GetByIdAsync(id);
            if (player == null) return NotFound();
            await _playerRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
