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
    public class TeamRepository : BaseRepository<Team>, ITeamRepository
    {
        #region Sobrecarga de Constructores (Constructor Overloading)

        public TeamRepository(ApplicationDbContext context) : base(context)
        {
        }

        public TeamRepository(ApplicationDbContext context, DbSet<Team> dbSet) : base(context, dbSet)
        {
        }

        #endregion

        #region Métodos Especializados de ITeamRepository

        public async Task<Team?> GetTeamWithPlayersAsync(int id)
        {
            return await _dbSet
                .Include(t => t.Players)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Team>> GetAllTeamsWithPlayersAsync()
        {
            return await _dbSet
                .Include(t => t.Players)
                .ToListAsync();
        }

        public async Task<IEnumerable<Team>> GetTeamsByCityAsync(string city)
        {
            return await _dbSet
                .Include(t => t.Players)
                .Where(t => t.City.ToLower() == city.ToLower())
                .ToListAsync();
        }

        #endregion
    }
}

