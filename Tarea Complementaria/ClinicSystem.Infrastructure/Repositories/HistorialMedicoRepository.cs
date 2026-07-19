using ClinicSystem.Domain.Entities;
using ClinicSystem.Domain.Interfaces;
using ClinicSystem.Infrastructure.Context;
using ClinicSystem.Infrastructure.Core;

namespace ClinicSystem.Infrastructure.Repositories
{
    public class HistorialMedicoRepository : BaseRepository<HistorialMedico>, IHistorialMedicoRepository
    {
        public HistorialMedicoRepository(ClinicDbContext context) : base(context)
        {
        }
    }
}
