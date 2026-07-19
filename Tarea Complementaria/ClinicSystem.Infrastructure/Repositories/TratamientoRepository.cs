using ClinicSystem.Domain.Entities;
using ClinicSystem.Domain.Interfaces;
using ClinicSystem.Infrastructure.Context;
using ClinicSystem.Infrastructure.Core;

namespace ClinicSystem.Infrastructure.Repositories
{
    public class TratamientoRepository : BaseRepository<Tratamiento>, ITratamientoRepository
    {
        public TratamientoRepository(ClinicDbContext context) : base(context)
        {
        }
    }
}
