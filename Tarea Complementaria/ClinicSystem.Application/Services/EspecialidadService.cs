using ClinicSystem.Application.Contract;
using ClinicSystem.Application.Dtos;
using ClinicSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicSystem.Application.Services
{
    public class EspecialidadService : IEspecialidadService
    {
        private readonly IEspecialidadRepository _especialidadRepository;

        public EspecialidadService(IEspecialidadRepository especialidadRepository)
        {
            _especialidadRepository = especialidadRepository;
        }

        public async Task<IEnumerable<EspecialidadDto>> GetAllEspecialidadesAsync()
        {
            var especialidades = await _especialidadRepository.GetAllAsync();
            return especialidades.Select(e => new EspecialidadDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Descripcion = e.Descripcion
            });
        }
    }
}
