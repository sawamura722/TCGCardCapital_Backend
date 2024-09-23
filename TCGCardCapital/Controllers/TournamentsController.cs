using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TCGCardCapital.DTOs;
using TCGCardCapital.Models;
using TCGCardCapital.Services.IService;
using TCGCardCapital.Services.ServiceImpl;

namespace TCGCardCapital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;

        public TournamentsController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDTO>>> GetTournaments()
        {
            var tournaments = await _tournamentService.GetTournamentsAsync();
            return Ok(tournaments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDTO>> GetTournament(int id)
        {
            var tournament = await _tournamentService.GetTournamentByIdAsync(id);

            if (tournament == null)
            {
                return NotFound();
            }

            return Ok(tournament);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<ActionResult<Tournament>> PostTournament([FromBody] TournamentDTO tournamentDTO)
        {
            var createdTournament = await _tournamentService.CreateTournamentAsync(tournamentDTO);
            return CreatedAtAction(nameof(GetTournament), new { id = createdTournament.TournamentId }, createdTournament);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(int id, [FromBody] TournamentDTO tournamentDTO)
        {
            if (await _tournamentService.UpdateTournamentAsync(id, tournamentDTO))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            if (await _tournamentService.DeleteTournamentAsync(id))
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
