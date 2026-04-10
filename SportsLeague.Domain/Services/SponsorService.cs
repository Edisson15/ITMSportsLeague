using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITournamentSponsorRepository _tournamentSponsorRepository;

        public SponsorService(
            ISponsorRepository sponsorRepository,
            ITournamentRepository tournamentRepository,
            ITournamentSponsorRepository tournamentSponsorRepository)
        {
            _sponsorRepository = sponsorRepository;
            _tournamentRepository = tournamentRepository;
            _tournamentSponsorRepository = tournamentSponsorRepository;
        }

        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            return await _sponsorRepository.GetAllAsync();
        }

        public async Task<Sponsor> GetByIdAsync(int id)
        {
            var sponsor = await _sponsorRepository.GetByIdAsync(id);

            if (sponsor == null)
                throw new KeyNotFoundException("Sponsor not found");

            return sponsor;
        }

        public async Task<Sponsor> CreateAsync(Sponsor sponsor)
        {
            if (await _sponsorRepository.ExistsByNameAsync(sponsor.Name))
                throw new InvalidOperationException("Sponsor name already exists");

            if (!IsValidEmail(sponsor.ContactEmail))
                throw new InvalidOperationException("Invalid email format");

            sponsor.CreatedAt = DateTime.UtcNow;

            return await _sponsorRepository.CreateAsync(sponsor);
        }

        public async Task<Sponsor> UpdateAsync(int id, Sponsor sponsor)
        {
            var existing = await _sponsorRepository.GetByIdAsync(id);

            if (existing == null)
                throw new KeyNotFoundException("Sponsor not found");

            existing.Name = sponsor.Name;
            existing.ContactEmail = sponsor.ContactEmail;
            existing.Phone = sponsor.Phone;
            existing.WebsiteUrl = sponsor.WebsiteUrl;
            existing.Category = sponsor.Category;
            existing.UpdatedAt = DateTime.UtcNow;

            await _sponsorRepository.UpdateAsync(existing);

            return existing;
        }

        public async Task DeleteAsync(int id)
        {
            var sponsor = await _sponsorRepository.GetByIdAsync(id);

            if (sponsor == null)
                throw new KeyNotFoundException("Sponsor not found");

            await _sponsorRepository.DeleteAsync(id);
        }

        public async Task LinkSponsorToTournamentAsync(int sponsorId, int tournamentId, decimal contractAmount)
        {
            if (contractAmount <= 0)
                throw new InvalidOperationException("ContractAmount must be greater than 0");

            var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);
            if (sponsor == null)
                throw new KeyNotFoundException("Sponsor not found");

            var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
            if (tournament == null)
                throw new KeyNotFoundException("Tournament not found");

            if (await _tournamentSponsorRepository.ExistsAsync(sponsorId, tournamentId))
                throw new InvalidOperationException("Sponsor already linked to this tournament");

            var relation = new TournamentSponsor
            {
                SponsorId = sponsorId,
                TournamentId = tournamentId,
                ContractAmount = contractAmount,
                JoinedAt = DateTime.UtcNow
            };

            await _tournamentSponsorRepository.CreateAsync(relation);
        }

        public async Task<IEnumerable<TournamentSponsor>> GetTournamentsBySponsorAsync(int sponsorId)
        {
            return await _tournamentSponsorRepository.GetBySponsorIdAsync(sponsorId);
        }

        public async Task UnlinkSponsorFromTournamentAsync(int sponsorId, int tournamentId)
        {
            var relation = await _tournamentSponsorRepository.GetRelationAsync(sponsorId, tournamentId);

            if (relation == null)
                throw new KeyNotFoundException("Relation not found");

            await _tournamentSponsorRepository.DeleteAsync(relation.Id);
        }

        private bool IsValidEmail(string email)
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }
    }
}
