using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces;
using SportsLeague.Infrastructure.Context;
using SportsLeague.Infrastructure.Core;

namespace SportsLeague.Infrastructure.Repositories
{
    public class PlayerRepository : BaseRepository<Player>, IPlayerRepository
    {
        public PlayerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
