namespace SportsLeague.API.DTOs.Response;

public class MatchLineupResponseDTO
{
    public int Id { get; set; }

    public int MatchId { get; set; }

    public int PlayerId { get; set; }

    public string PlayerName { get; set; }

    public string TeamName { get; set; }

    public bool IsStarter { get; set; }

    public string Position { get; set; }
}