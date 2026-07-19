using ClinicSystem.Domain.Entities;
using ClinicSystem.Domain.Interfaces;
using ClinicSystem.Infrastructure.Context;
using ClinicSystem.Infrastructure.Core;

namespace ClinicSystem.Infrastructure.Repositories
{
    public class MedicamentoRepository : BaseRepository<Medicamento>, IMedicamentoRepository
    {
        public MedicamentoRepository(ClinicDbContext context) : base(context)
        {
        }
    }
}
