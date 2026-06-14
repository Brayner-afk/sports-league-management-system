using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsLeagueApi.Data;
using SportsLeagueApi.DTOs;
using SportsLeagueApi.Models;

namespace SportsLeagueApi.Controllers {
    /// <summary>
    /// Gestión de Jugadores de la Liga
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase {
        private readonly ApplicationDbContext _context;
        public PlayersController(ApplicationDbContext context) { _context = context; }

        /// <summary>
        /// Obtener lista de todos los jugadores registrados
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDTO>>> GetPlayers() {
            var players = await _context.Players.ToListAsync();
            return Ok(players.Select(p => new PlayerDTO { Id = p.Id, FullName = p.FullName, Position = p.Position, TeamId = p.TeamId }));
        }

        /// <summary>
        /// Buscar un jugador específico por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDTO>> GetPlayer(int id) {
            var player = await _context.Players.FindAsync(id);
            if (player == null) return NotFound();
            return Ok(new PlayerDTO { Id = player.Id, FullName = player.FullName, Position = player.Position, TeamId = player.TeamId });
        }

        /// <summary>
        /// Agregar un nuevo jugador y asignarlo a un equipo
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PlayerDTO>> PostPlayer(PlayerDTO playerDto) {
            if (!await _context.Teams.AnyAsync(t => t.Id == playerDto.TeamId)) return BadRequest("El TeamId no existe.");
            var player = new Player { FullName = playerDto.FullName, Position = playerDto.Position, TeamId = playerDto.TeamId };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            playerDto.Id = player.Id;
            return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, playerDto);
        }

        /// <summary>
        /// Actualizar los datos de un jugador existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, PlayerDTO playerDto) {
            if (id != playerDto.Id) return BadRequest();
            var player = await _context.Players.FindAsync(id);
            if (player == null) return NotFound();
            if (!await _context.Teams.AnyAsync(t => t.Id == playerDto.TeamId)) return BadRequest("El TeamId no existe.");
            player.FullName = playerDto.FullName; player.Position = playerDto.Position; player.TeamId = playerDto.TeamId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Eliminar un jugador del sistema
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id) {
            var player = await _context.Players.FindAsync(id);
            if (player == null) return NotFound();
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
