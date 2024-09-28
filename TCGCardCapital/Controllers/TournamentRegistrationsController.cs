using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TCGCardCapital.DTOs;
using TCGCardCapital.Services.IService;

namespace TCGCardCapital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentRegistrationsController : ControllerBase
    {
        private readonly ITournamentRegistrationService _tournamentRegistrationService;

        public TournamentRegistrationsController(ITournamentRegistrationService tournamentRegistrationService)
        {
            _tournamentRegistrationService = tournamentRegistrationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentRegistrationDTO>>> GetAllTournamentUsers()
        {
            var users = await _tournamentRegistrationService.GetAllTournamentUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = "USER")]
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<TournamentRegistrationDTO>>> GetTournamentUsersByUserId(int userId)
        {
            var users = await _tournamentRegistrationService.GetTournamentUsersByUserIdAsync(userId);
            return Ok(users);
        }

        [Authorize(Roles = "USER")]
        [HttpPost]
        public async Task<IActionResult> RegisterUserForTournament([FromForm] TournamentRegistrationDTO registrationDTO)
        {
            var result = await _tournamentRegistrationService.RegisterUserForTournamentAsync(registrationDTO);
            if (result)
            {
                return Ok("User registered for the tournament successfully.");
            }

            return BadRequest("User is already registered for this tournament.");
        }

        [Authorize(Roles = "ADMIN")]
        [Authorize(Roles = "USER")]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteTournamentRegistrationByUserId(int userId)
        {
            var result = await _tournamentRegistrationService.DeleteTournamentRegistrationByUserIdAsync(userId);
            if (result)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
