using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.DataAccess.Repositories
{
    public class TournamentSponsorRepository : ITournamentSponsorRepository
    {
        private readonly LeagueDbContext _context;

        public TournamentSponsorRepository(LeagueDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TournamentSponsor entity)
        {
            await _context.TournamentSponsors.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int sponsorId, int tournamentId)
        {
            return await _context.TournamentSponsors
                .AnyAsync(x => x.SponsorId == sponsorId && x.TournamentId == tournamentId);
        }

        public async Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId)
        {
            return await _context.TournamentSponsors
                .Include(ts => ts.Sponsor)
                .Include(ts => ts.Tournament)
                .Where(ts => ts.SponsorId == sponsorId)
                .ToListAsync();
        }

        public async Task<TournamentSponsor?> GetRelationAsync(int sponsorId, int tournamentId)
        {
            return await _context.TournamentSponsors
                .FirstOrDefaultAsync(x => x.SponsorId == sponsorId && x.TournamentId == tournamentId);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.TournamentSponsors.FindAsync(id);

            if (entity != null)
            {
                _context.TournamentSponsors.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
