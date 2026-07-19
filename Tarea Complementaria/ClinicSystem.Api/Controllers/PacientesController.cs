using ClinicSystem.Application.Contract;
using ClinicSystem.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ClinicSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;

        public PacientesController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PacienteDto>>> GetAll()
        {
            return Ok(await _pacienteService.GetAllPacientesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PacienteDto>> GetById(int id)
        {
            var p = await _pacienteService.GetPacienteByIdAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpPost]
        public async Task<ActionResult<PacienteDto>> Create(PacienteDto dto)
        {
            try
            {
                var created = await _pacienteService.CreatePacienteAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PacienteDto dto)
        {
            if (id != dto.Id) return BadRequest("El ID de la URL no coincide con el del cuerpo.");
            try
            {
                var updated = await _pacienteService.UpdatePacienteAsync(id, dto);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _pacienteService.DeletePacienteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
