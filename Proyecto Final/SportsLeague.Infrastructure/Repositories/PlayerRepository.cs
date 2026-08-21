using Microsoft.EntityFrameworkCore;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces;
using SportsLeague.Infrastructure.Context;
using SportsLeague.Infrastructure.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsLeague.Infrastructure.Repositories
{
    public class PlayerRepository : BaseRepository<Player>, IPlayerRepository
    {
        #region Sobrecarga de Constructores (Constructor Overloading)

        public PlayerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public PlayerRepository(ApplicationDbContext context, DbSet<Player> dbSet) : base(context, dbSet)
        {
        }

        #endregion

        #region Métodos Especializados de IPlayerRepository

        public async Task<IEnumerable<Player>> GetPlayersByTeamIdAsync(int teamId)
        {
            return await _dbSet
                .Include(p => p.Team)
                .Where(p => p.TeamId == teamId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Player>> GetAllPlayersWithTeamAsync()
        {
            return await _dbSet
                .Include(p => p.Team)
                .ToListAsync();
        }

        public async Task<Player?> GetPlayerWithTeamAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Team)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Player>> GetPlayersByPositionAsync(string position)
        {
            return await _dbSet
                .Include(p => p.Team)
                .Where(p => p.Position.ToLower() == position.ToLower())
                .ToListAsync();
        }

        #endregion
    }
}

