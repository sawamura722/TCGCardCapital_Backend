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
        [Authorize(Roles = "USER")]
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

        [Authorize(Roles = "ADMIN")]
        [Authorize(Roles = "USER")]
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutUserProfile(int userId, [FromForm] UserProfileDTO userProfileDTO)
        {
            if (await _userProfileService.UpdateUserProfileAsync(userId, userProfileDTO))
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
