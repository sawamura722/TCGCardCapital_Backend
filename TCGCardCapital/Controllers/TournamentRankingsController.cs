
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TCGCardCapital.DTOs;
using TCGCardCapital.Services.IService;
using TCGCardCapital.Services.ServiceImpl;

namespace TCGCardCapital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentRankingsController : ControllerBase
    {
        private readonly ITournamentRankingService _tournamentRankingService;

        public TournamentRankingsController(ITournamentRankingService tournamentRankingService)
        {
            _tournamentRankingService = tournamentRankingService;
        }

        // Get rankings by tournament id
        [HttpGet("{tournamentId}")]
        public async Task<ActionResult<IEnumerable<TournamentRankingDTO>>> GetRankingByTournamentId(int tournamentId)
        {
            var rankings = await _tournamentRankingService.GetRankingByTournamentIdAsync(tournamentId);
            return Ok(rankings);
        }

        // Update user rank by tournament id and user id
        [Authorize(Roles = "ADMIN")]
        [HttpPut("{tournamentId}/{userId}")]
        public async Task<IActionResult> UpdateUserRank(int tournamentId, int userId, int rank)
        {
            var result = await _tournamentRankingService.UpdateUserRankAsync(tournamentId, userId, rank);
            if (result)
            {
                return NoContent(); 
            }

            return NotFound(); 
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{tournamentId}")]
        public async Task<IActionResult> DeleteTournamentRankingsByTournamentId(int tournamentId)
        {
            var result = await _tournamentRankingService.DeleteRankingsByTournamentIdAsync(tournamentId);
            if (result)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
