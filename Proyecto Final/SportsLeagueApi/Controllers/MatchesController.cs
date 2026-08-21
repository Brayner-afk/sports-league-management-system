using Microsoft.AspNetCore.Mvc;
using SportsLeague.Application.Contract;
using SportsLeague.Application.Dtos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SportsLeagueApi.Controllers
{
    /// <summary>
    /// Gestión de Partidos y Calendario de la Liga Deportiva
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchesController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        /// <summary>
        /// Obtener lista de partidos con filtros opcionales de estado y equipo
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchDto>>> GetMatches([FromQuery] string? status = null, [FromQuery] int? teamId = null)
        {
            if (teamId.HasValue && teamId.Value > 0)
            {
                var matchesByTeam = await _matchService.GetMatchesByTeamAsync(teamId.Value);
                return Ok(matchesByTeam);
            }

            var matches = await _matchService.GetAllMatchesAsync(status);
            return Ok(matches);
        }

        /// <summary>
        /// Obtener un partido por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MatchDto>> GetMatch(int id)
        {
            var match = await _matchService.GetMatchByIdAsync(id);
            if (match == null) return NotFound();
            return Ok(match);
        }

        /// <summary>
        /// Programar un nuevo partido en el calendario
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<MatchDto>> PostMatch(MatchDto matchDto)
        {
            try
            {
                var createdMatch = await _matchService.CreateMatchAsync(matchDto);
                return CreatedAtAction(nameof(GetMatch), new { id = createdMatch.Id }, createdMatch);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualizar datos, fecha, sede o marcador de un partido
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatch(int id, MatchDto matchDto)
        {
            if (id != matchDto.Id) return BadRequest(new { message = "El Id de la URL no coincide con el del cuerpo." });
            try
            {
                var updated = await _matchService.UpdateMatchAsync(id, matchDto);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Eliminar un partido del calendario
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            var deleted = await _matchService.DeleteMatchAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}