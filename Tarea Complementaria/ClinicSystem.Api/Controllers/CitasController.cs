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
    public class CitasController : ControllerBase
    {
        private readonly ICitaService _citaService;

        public CitasController(ICitaService citaService)
        {
            _citaService = citaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CitaDto>>> GetAll()
        {
            return Ok(await _citaService.GetAllCitasAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CitaDto>> GetById(int id)
        {
            var c = await _citaService.GetCitaByIdAsync(id);
            if (c == null) return NotFound();
            return Ok(c);
        }

        [HttpPost]
        public async Task<ActionResult<CitaDto>> Create(CitaDto dto)
        {
            try
            {
                var created = await _citaService.CreateCitaAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CitaDto dto)
        {
            if (id != dto.Id) return BadRequest("El ID de la URL no coincide con el del cuerpo.");
            try
            {
                var updated = await _citaService.UpdateCitaAsync(id, dto);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (ValidationException ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _citaService.DeleteCitaAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
