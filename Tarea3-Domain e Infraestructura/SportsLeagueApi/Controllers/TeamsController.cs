using Microsoft.AspNetCore.Mvc;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces;
using SportsLeague.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsLeagueApi.Controllers {
    /// <summary>
    /// Gestión de Equipos de la Liga
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase {
        private readonly ITeamRepository _teamRepository;

        public TeamsController(ITeamRepository teamRepository) {
            _teamRepository = teamRepository;
        }

        /// <summary>
        /// Obtener lista de todos los equipos registrados
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetTeams() {
            var teams = await _teamRepository.GetAllAsync();
            return Ok(teams.Select(t => new TeamDto { Id = t.Id, Name = t.Name, City = t.City }));
        }

        /// <summary>
        /// Buscar un equipo específico por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDto>> GetTeam(int id) {
            var team = await _teamRepository.GetByIdAsync(id);
            if (team == null) return NotFound();
            return Ok(new TeamDto { Id = team.Id, Name = team.Name, City = team.City });
        }

        /// <summary>
        /// Agregar un nuevo equipo al sistema
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TeamDto>> PostTeam(TeamDto teamDto) {
            var team = new Team { Name = teamDto.Name, City = teamDto.City };
            await _teamRepository.AddAsync(team);
            teamDto.Id = team.Id;
            return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, teamDto);
        }

        /// <summary>
        /// Actualizar los datos de un equipo existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, TeamDto teamDto) {
            if (id != teamDto.Id) return BadRequest();
            var team = await _teamRepository.GetByIdAsync(id);
            if (team == null) return NotFound();
            
            team.Name = teamDto.Name;
            team.City = teamDto.City;
            await _teamRepository.UpdateAsync(team);
            return NoContent();
        }

        /// <summary>
        /// Eliminar un equipo del sistema
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id) {
            var team = await _teamRepository.GetByIdAsync(id);
            if (team == null) return NotFound();
            await _teamRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
