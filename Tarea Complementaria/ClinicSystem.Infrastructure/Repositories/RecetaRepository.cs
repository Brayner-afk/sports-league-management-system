using ClinicSystem.Domain.Entities;
using ClinicSystem.Domain.Interfaces;
using ClinicSystem.Infrastructure.Context;
using ClinicSystem.Infrastructure.Core;

namespace ClinicSystem.Infrastructure.Repositories
{
    public class RecetaRepository : BaseRepository<Receta>, IRecetaRepository
    {
        public RecetaRepository(ClinicDbContext context) : base(context)
        {
        }
    }
}
