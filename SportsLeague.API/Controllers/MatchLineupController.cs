using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers
{
    [ApiController]
    [Route("api/match/{matchId}/lineup")]
    public class MatchLineupController : ControllerBase
    {
        private readonly IMatchLineupService _matchLineupService;
        private readonly IMapper _mapper;

        public MatchLineupController(
            IMatchLineupService matchLineupService,
            IMapper mapper)
        {
            _matchLineupService = matchLineupService;
            _mapper = mapper;
        }

        // POST: api/match/{matchId}/lineup
        [HttpPost]
        public async Task<ActionResult<MatchLineupResponseDTO>> Create(
            int matchId,
            MatchLineupRequestDTO dto)
        {
            try
            {
                var lineup = _mapper.Map<MatchLineup>(dto);

                lineup.MatchId = matchId;

                var created = await _matchLineupService.CreateAsync(lineup);

                var response = _mapper.Map<MatchLineupResponseDTO>(created);

                return CreatedAtAction(
                    nameof(GetByMatch),
                    new { matchId = created.MatchId },
                    response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        // GET: api/match/{matchId}/lineup
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchLineupResponseDTO>>> GetByMatch(
            int matchId)
        {
            var lineups = await _matchLineupService.GetByMatchAsync(matchId);

            var response = _mapper.Map<IEnumerable<MatchLineupResponseDTO>>(lineups);

            return Ok(response);
        }

        // GET: api/match/{matchId}/lineup/team/{teamId}
        [HttpGet("team/{teamId}")]
        public async Task<ActionResult<IEnumerable<MatchLineupResponseDTO>>> GetByMatchAndTeam(
            int matchId,
            int teamId)
        {
            var lineups = await _matchLineupService.GetByMatchAndTeamAsync(
                matchId,
                teamId);

            var response = _mapper.Map<IEnumerable<MatchLineupResponseDTO>>(lineups);

            return Ok(response);
        }

        // DELETE: api/match/{matchId}/lineup/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int matchId,
            int id)
        {
            try
            {
                await _matchLineupService.DeleteAsync(matchId, id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }
    }
}