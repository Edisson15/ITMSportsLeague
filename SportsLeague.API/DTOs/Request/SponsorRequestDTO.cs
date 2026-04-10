using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Request
{
    public class SponsorRequestDTO
    {
        public string Name { get; set; }

        public string ContactEmail { get; set; }

        public string? Phone { get; set; }

        public string? WebsiteUrl { get; set; }

        public SponsorCategory Category { get; set; }
    }
}
