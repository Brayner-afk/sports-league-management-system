using ClinicSystem.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicSystem.Application.Contract
{
    public interface IMedicoService
    {
        Task<IEnumerable<MedicoDto>> GetAllMedicosAsync();
        Task<MedicoDto?> GetMedicoByIdAsync(int id);
    }
}
