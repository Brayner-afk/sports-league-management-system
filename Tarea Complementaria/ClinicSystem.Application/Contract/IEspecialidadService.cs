using ClinicSystem.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicSystem.Application.Contract
{
    public interface IEspecialidadService
    {
        Task<IEnumerable<EspecialidadDto>> GetAllEspecialidadesAsync();
    }
}
