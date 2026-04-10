using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SponsorController : ControllerBase
    {
        private readonly ISponsorService _sponsorService;
        private readonly IMapper _mapper;

        public SponsorController(ISponsorService sponsorService, IMapper mapper)
        {
            _sponsorService = sponsorService;
            _mapper = mapper;
        }

        // GET api/Sponsor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SponsorResponseDTO>>> GetAll()
        {
            var sponsors = await _sponsorService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<SponsorResponseDTO>>(sponsors));
        }

        // GET api/Sponsor/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SponsorResponseDTO>> GetById(int id)
        {
            try
            {
                var sponsor = await _sponsorService.GetByIdAsync(id);
                return Ok(_mapper.Map<SponsorResponseDTO>(sponsor));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // POST api/Sponsor
        [HttpPost]
        public async Task<ActionResult<SponsorResponseDTO>> Create(SponsorRequestDTO dto)
        {
            try
            {
                var sponsor = _mapper.Map<Sponsor>(dto);
                var createdSponsor = await _sponsorService.CreateAsync(sponsor);
                var response = _mapper.Map<SponsorResponseDTO>(createdSponsor);

                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // PUT api/Sponsor/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SponsorRequestDTO dto)
        {
            try
            {
                var sponsor = _mapper.Map<Sponsor>(dto);
                await _sponsorService.UpdateAsync(id, sponsor);

                return NoContent(); // 204 ✔
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // DELETE api/Sponsor/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sponsorService.DeleteAsync(id);
                return NoContent(); // 204 ✔
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // GET api/Sponsor/{id}/tournaments
        [HttpGet("{id}/tournaments")]
        public async Task<ActionResult<IEnumerable<TournamentSponsorResponseDTO>>> GetSponsorTournaments(int id)
        {
            try
            {
                var relations = await _sponsorService.GetTournamentsBySponsorAsync(id);

                return Ok(_mapper.Map<IEnumerable<TournamentSponsorResponseDTO>>(relations)); // 200 ✔
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // POST api/Sponsor/{id}/tournaments
        [HttpPost("{id}/tournaments")]
        public async Task<IActionResult> LinkTournament(int id, TournamentSponsorRequestDTO dto)
        {
            try
            {
                await _sponsorService.LinkSponsorToTournamentAsync(
                    id,
                    dto.TournamentId,
                    dto.ContractAmount
                );

                return StatusCode(201, new
                {
                    sponsorId = id,
                    tournamentId = dto.TournamentId,
                    contractAmount = dto.ContractAmount
                }); // 201 ✔
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); // 404 ✔
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); // 409 ✔
            }
        }

        // DELETE api/Sponsor/{id}/tournaments/{tournamentId}
        [HttpDelete("{id}/tournaments/{tournamentId}")]
        public async Task<IActionResult> UnlinkTournament(int id, int tournamentId)
        {
            try
            {
                await _sponsorService.UnlinkSponsorFromTournamentAsync(id, tournamentId);

                return NoContent(); // 204 ✔
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); // 404 ✔
            }
        }
    }
}
