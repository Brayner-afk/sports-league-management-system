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
    public class MatchRepository : BaseRepository<Match>, IMatchRepository
    {
        public MatchRepository(ApplicationDbContext context) : base(context)
        {
        }

        public MatchRepository(ApplicationDbContext context, DbSet<Match> dbSet) : base(context, dbSet)
        {
        }

        public async Task<IEnumerable<Match>> GetAllMatchesWithTeamsAsync()
        {
            return await _dbSet
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .OrderBy(m => m.MatchDate)
                .ToListAsync();
        }

        public async Task<Match?> GetMatchWithTeamsByIdAsync(int id)
        {
            return await _dbSet
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Match>> GetMatchesByTeamIdAsync(int teamId)
        {
            return await _dbSet
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Where(m => m.HomeTeamId == teamId || m.AwayTeamId == teamId)
                .OrderBy(m => m.MatchDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Match>> GetMatchesByStatusAsync(string status)
        {
            return await _dbSet
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Where(m => m.Status.ToLower() == status.ToLower())
                .OrderBy(m => m.MatchDate)
                .ToListAsync();
        }
    }
}