using ClinicSystem.Application.Contract;
using ClinicSystem.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EspecialidadesController : ControllerBase
    {
        private readonly IEspecialidadService _especialidadService;

        public EspecialidadesController(IEspecialidadService especialidadService)
        {
            _especialidadService = especialidadService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EspecialidadDto>>> GetAll()
        {
            return Ok(await _especialidadService.GetAllEspecialidadesAsync());
        }
    }
}
