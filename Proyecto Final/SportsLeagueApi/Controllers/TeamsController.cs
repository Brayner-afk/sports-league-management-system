using Microsoft.AspNetCore.Mvc;
using SportsLeague.Application.Contract;
using SportsLeague.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SportsLeagueApi.Controllers {
    /// <summary>
    /// Gestión de Equipos de la Liga Deportiva
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService) {
            _teamService = teamService;
        }

        /// <summary>
        /// Obtener lista de todos los equipos registrados, con soporte para filtrado por ciudad.
        /// </summary>
        /// <param name="city">Filtro opcional por ciudad</param>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetTeams([FromQuery] string? city = null) {
            var teams = await _teamService.GetAllTeamsAsync(city);
            return Ok(teams);
        }

        /// <summary>
        /// Buscar un equipo específico por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDto>> GetTeam(int id) {
            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null) return NotFound();
            return Ok(team);
        }

        /// <summary>
        /// Agregar un nuevo equipo al sistema
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TeamDto>> PostTeam(TeamDto teamDto) {
            try {
                var createdTeam = await _teamService.CreateTeamAsync(teamDto);
                return CreatedAtAction(nameof(GetTeam), new { id = createdTeam.Id }, createdTeam);
            }
            catch (ValidationException ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualizar los datos de un equipo existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, TeamDto teamDto) {
            if (id != teamDto.Id) return BadRequest(new { message = "El Id de la URL no coincide con el del cuerpo." });
            try {
                var updated = await _teamService.UpdateTeamAsync(id, teamDto);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (ValidationException ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Eliminar un equipo del sistema
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id) {
            var deleted = await _teamService.DeleteTeamAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}

