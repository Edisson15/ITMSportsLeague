using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;


namespace SportsLeague.Domain.Services;

public class MatchLineupService : IMatchLineupService
{
    private readonly IMatchLineupRepository _lineupRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly IPlayerRepository _playerRepository;

    public MatchLineupService(
        IMatchLineupRepository lineupRepository,
        IMatchRepository matchRepository,
        IPlayerRepository playerRepository)
    {
        _lineupRepository = lineupRepository;
        _matchRepository = matchRepository;
        _playerRepository = playerRepository;
    }

    public async Task<MatchLineup> CreateAsync(MatchLineup lineup)
    {
        // V1
        var match = await _matchRepository.GetByIdAsync(lineup.MatchId);

        if (match == null)
            throw new KeyNotFoundException(
                $"No se encontró el partido con ID {lineup.MatchId}");

        // V6
        if (match.Status != MatchStatus.Scheduled)
            throw new InvalidOperationException(
                "Solo se pueden registrar alineaciones en partidos Scheduled");

        // V2
        var player = await _playerRepository.GetByIdAsync(lineup.PlayerId);

        if (player == null)
            throw new KeyNotFoundException(
                $"No se encontró el jugador con ID {lineup.PlayerId}");

        // V3
        if (player.TeamId != match.HomeTeamId &&
            player.TeamId != match.AwayTeamId)
        {
            throw new InvalidOperationException(
                "El jugador no pertenece a ninguno de los equipos del partido");
        }

        // V4
        var exists = await _lineupRepository
            .ExistsByMatchAndPlayerAsync(
                lineup.MatchId,
                lineup.PlayerId);

        if (exists)
            throw new InvalidOperationException(
                "El jugador ya está registrado en la alineación de este partido");

        // V5
        if (lineup.IsStarter)
        {
            var starters = await _lineupRepository
                .CountStartersAsync(
                    lineup.MatchId,
                    player.TeamId);

            if (starters >= 11)
                throw new InvalidOperationException(
                    "El equipo ya tiene 11 titulares registrados en este partido");
        }

        return await _lineupRepository.CreateAsync(lineup);
    }

    public async Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId)
    {
        return await _lineupRepository.GetByMatchAsync(matchId);
    }

    public async Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(
        int matchId,
        int teamId)
    {
        return await _lineupRepository
            .GetByMatchAndTeamAsync(matchId, teamId);
    }

    public async Task DeleteAsync(int matchId, int id)
    {
        var lineup = await _lineupRepository.GetByIdAsync(id);

        if (lineup == null || lineup.MatchId != matchId)
            throw new KeyNotFoundException(
                "No se encontró la alineación");

        await _lineupRepository.DeleteAsync(lineup);
    }
}