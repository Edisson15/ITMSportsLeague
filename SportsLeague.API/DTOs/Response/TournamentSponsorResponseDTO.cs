namespace SportsLeague.API.DTOs.Response
{
    public class TournamentSponsorResponseDTO
    {
        public int SponsorId { get; set; }

        public int TournamentId { get; set; }

        public string SponsorName { get; set; }

        public string TournamentName { get; set; }
    }
}
