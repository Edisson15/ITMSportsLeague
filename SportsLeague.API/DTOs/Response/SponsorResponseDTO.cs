using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Response
{
    public class SponsorResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ContactEmail { get; set; }

        public string? Phone { get; set; }

        public string? WebsiteUrl { get; set; }

        public SponsorCategory Category { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
