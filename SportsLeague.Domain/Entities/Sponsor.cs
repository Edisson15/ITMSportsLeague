using SportsLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Entities
{
    public class Sponsor : AuditBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ContactEmail { get; set; }

        public string? Phone { get; set; }

        public string? WebsiteUrl { get; set; }

        public SponsorCategory Category { get; set; }

        public ICollection<TournamentSponsor> TournamentSponsors { get; set; }
    }
}
