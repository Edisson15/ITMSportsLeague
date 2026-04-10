using SportsLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ITournamentSponsorRepository
    {
        Task CreateAsync(TournamentSponsor entity);

        Task<bool> ExistsAsync(int sponsorId, int tournamentId);

        Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId);

        Task<TournamentSponsor?> GetRelationAsync(int sponsorId, int tournamentId);

        Task DeleteAsync(int id);
    }
}
