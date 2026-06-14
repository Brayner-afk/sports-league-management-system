using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsLeagueApi.Data;
using SportsLeagueApi.DTOs;
using SportsLeagueApi.Models;

namespace SportsLeagueApi.Controllers {
    /// <summary>
    /// Gestión de Equipos de la Liga
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase {
        private readonly ApplicationDbContext _context;
        public TeamsController(ApplicationDbContext context) { _context = context; }

        /// <summary>
        /// Obtener lista de todos los equipos registrados
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetTeams() {
            var teams = await _context.Teams.ToListAsync();
            return Ok(teams.Select(t => new TeamDTO { Id = t.Id, Name = t.Name, City = t.City }));
        }

        /// <summary>
        /// Buscar un equipo específico por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDTO>> GetTeam(int id) {
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return NotFound();
            return Ok(new TeamDTO { Id = team.Id, Name = team.Name, City = team.City });
        }

        /// <summary>
        /// Agregar un nuevo equipo al sistema
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TeamDTO>> PostTeam(TeamDTO teamDto) {
            var team = new Team { Name = teamDto.Name, City = teamDto.City };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            teamDto.Id = team.Id;
            return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, teamDto);
        }

        /// <summary>
        /// Actualizar los datos de un equipo existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, TeamDTO teamDto) {
            if (id != teamDto.Id) return BadRequest();
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return NotFound();
            team.Name = teamDto.Name; team.City = teamDto.City;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Eliminar un equipo del sistema
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id) {
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return NotFound();
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
