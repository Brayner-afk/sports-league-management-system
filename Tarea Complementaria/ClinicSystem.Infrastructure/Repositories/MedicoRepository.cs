using ClinicSystem.Domain.Entities;
using ClinicSystem.Domain.Interfaces;
using ClinicSystem.Infrastructure.Context;
using ClinicSystem.Infrastructure.Core;

namespace ClinicSystem.Infrastructure.Repositories
{
    public class MedicoRepository : BaseRepository<Medico>, IMedicoRepository
    {
        public MedicoRepository(ClinicDbContext context) : base(context)
        {
        }
    }
}
