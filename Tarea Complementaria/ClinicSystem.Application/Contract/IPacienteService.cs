using ClinicSystem.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicSystem.Application.Contract
{
    public interface IPacienteService
    {
        Task<IEnumerable<PacienteDto>> GetAllPacientesAsync();
        Task<PacienteDto?> GetPacienteByIdAsync(int id);
        Task<PacienteDto> CreatePacienteAsync(PacienteDto dto);
        Task<bool> UpdatePacienteAsync(int id, PacienteDto dto);
        Task<bool> DeletePacienteAsync(int id);
    }
}
