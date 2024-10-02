using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TCGCardCapital.DTOs;
using TCGCardCapital.Services.IService;
using TCGCardCapital.Services.ServiceImpl;

namespace TCGCardCapital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfilesController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfilesController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfileDTO>>> GetUsers()
        {
            var users = await _userProfileService.GetUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = "ADMIN,USER")]
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserProfileDTO>> GetUserProfile(int userId)
        {
            var user = await _userProfileService.GetUserProfileByUserIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize(Roles = "ADMIN,USER")]
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutUserProfile(int userId, [FromForm] UserProfileDTO userProfileDTO)
        {
            if (await _userProfileService.UpdateUserProfileAsync(userId, userProfileDTO))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [Authorize(Roles = "USER")]
        [HttpPut("sub/{userId}")]
        public async Task<IActionResult> BuySub(int userId)
        {
            if (await _userProfileService.BuySubscription(userId))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [Authorize(Roles = "ADMIN,USER")]
        [HttpPut("pointInc/{userId}")]
        public async Task<IActionResult> IncreasePoint(int userId, [FromForm] IncreasePointDTO increasePointDTO)
        {
            if (await _userProfileService.IncreasePoint(userId, increasePointDTO))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [Authorize(Roles = "ADMIN,USER")]
        [HttpPut("pointDec/{userId}")]
        public async Task<IActionResult> DecreasePoint(int userId, [FromForm] IncreasePointDTO increasePointDTO)
        {
            if (await _userProfileService.DecreasePoint(userId, increasePointDTO))
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
