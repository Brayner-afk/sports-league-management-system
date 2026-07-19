using ClinicSystem.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicSystem.Application.Contract
{
    public interface ICitaService
    {
        Task<IEnumerable<CitaDto>> GetAllCitasAsync();
        Task<CitaDto?> GetCitaByIdAsync(int id);
        Task<CitaDto> CreateCitaAsync(CitaDto dto);
        Task<bool> UpdateCitaAsync(int id, CitaDto dto);
        Task<bool> DeleteCitaAsync(int id);
    }
}
