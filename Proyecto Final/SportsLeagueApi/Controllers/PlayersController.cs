using Microsoft.AspNetCore.Mvc;
using SportsLeague.Application.Contract;
using SportsLeague.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SportsLeagueApi.Controllers {
    /// <summary>
    /// Gestión de Jugadores de la Liga Deportiva
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService) {
            _playerService = playerService;
        }

        /// <summary>
        /// Obtener lista de todos los jugadores registrados, con soporte para filtrado por ID de equipo.
        /// </summary>
        /// <param name="teamId">Filtro opcional por equipo</param>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDto>>> GetPlayers([FromQuery] int? teamId = null) {
            var players = await _playerService.GetAllPlayersAsync(teamId);
            return Ok(players);
        }

        /// <summary>
        /// Buscar un jugador específico por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDto>> GetPlayer(int id) {
            var player = await _playerService.GetPlayerByIdAsync(id);
            if (player == null) return NotFound();
            return Ok(player);
        }

        /// <summary>
        /// Agregar un nuevo jugador y asignarlo a un equipo
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PlayerDto>> PostPlayer(PlayerDto playerDto) {
            try {
                var createdPlayer = await _playerService.CreatePlayerAsync(playerDto);
                return CreatedAtAction(nameof(GetPlayer), new { id = createdPlayer.Id }, createdPlayer);
            }
            catch (ValidationException ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualizar los datos de un jugador existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, PlayerDto playerDto) {
            if (id != playerDto.Id) return BadRequest(new { message = "El Id de la URL no coincide con el del cuerpo." });
            try {
                var updated = await _playerService.UpdatePlayerAsync(id, playerDto);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (ValidationException ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Eliminar un jugador del sistema
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id) {
            var deleted = await _playerService.DeletePlayerAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}

