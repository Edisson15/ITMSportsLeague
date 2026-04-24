using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.DataAccess.Repositories
{
    public class MatchResultRepository : GenericRepository<MatchResult>, IMatchResultRepository
    {
        public MatchResultRepository(LeagueDbContext context) : base(context) { }

        public async Task<MatchResult?> GetByMatchIdAsync(int matchId)
        {
            return await _dbSet
                .Where(mr => mr.MatchId == matchId)
                .FirstOrDefaultAsync();

        }
    }

}
