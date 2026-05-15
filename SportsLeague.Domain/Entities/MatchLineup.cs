using SportsLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

public class MatchLineup
{
    public int Id { get; set; }

    public int MatchId { get; set; }
    public Match Match { get; set; }

    public int PlayerId { get; set; }
    public Player Player { get; set; }

    public bool IsStarter { get; set; }

    public string Position { get; set; }
}
