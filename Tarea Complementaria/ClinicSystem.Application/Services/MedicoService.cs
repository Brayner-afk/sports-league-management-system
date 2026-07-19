using ClinicSystem.Application.Contract;
using ClinicSystem.Application.Dtos;
using ClinicSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicSystem.Application.Services
{
    public class MedicoService : IMedicoService
    {
        private readonly IMedicoRepository _medicoRepository;

        public MedicoService(IMedicoRepository medicoRepository)
        {
            _medicoRepository = medicoRepository;
        }

        public async Task<IEnumerable<MedicoDto>> GetAllMedicosAsync()
        {
            var medicos = await _medicoRepository.GetAllAsync();
            return medicos.Select(m => new MedicoDto
            {
                Id = m.Id,
                NombreCompleto = m.NombreCompleto,
                LicenciaMedica = m.LicenciaMedica,
                Telefono = m.Telefono,
                EspecialidadId = m.EspecialidadId
            });
        }

        public async Task<MedicoDto?> GetMedicoByIdAsync(int id)
        {
            var m = await _medicoRepository.GetByIdAsync(id);
            if (m == null) return null;

            return new MedicoDto
            {
                Id = m.Id,
                NombreCompleto = m.NombreCompleto,
                LicenciaMedica = m.LicenciaMedica,
                Telefono = m.Telefono,
                EspecialidadId = m.EspecialidadId
            };
        }
    }
}
