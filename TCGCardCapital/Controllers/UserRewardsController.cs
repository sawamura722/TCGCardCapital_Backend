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
    public class UserRewardsController : ControllerBase
    {
        private readonly IUserRewardService _userRewardService;

        public UserRewardsController(IUserRewardService userRewardService)
        {
            _userRewardService = userRewardService;
        }

        [Authorize(Roles = "ADMIN,USER")]
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<UserRewardDTO>>> GetClaimRewardsByUserId(int userId)
        {
            var rewards = await _userRewardService.GetClaimedRewardsByUserIdAsync(userId);
            return Ok(rewards);
        }

        [Authorize(Roles = "USER")]
        [HttpGet("{userId}/{rewardId}")]
        public async Task<ActionResult<UserRewardDTO>> GetClaimRewardByUserId(int userId, int rewardId)
        {
            var rewards = await _userRewardService.GetClaimedRewardByUserIdAsync(userId, rewardId);
            return Ok(rewards);
        }

        [Authorize(Roles = "ADMIN,USER")]
        [HttpPost]
        public async Task<ActionResult<RewardDTO>> PostClaimReward([FromForm] UserRewardDTO userRewardDTO)
        {
            try
            {
                var createdUserReward = await _userRewardService.CreateUserRewardAsync(userRewardDTO);
                return CreatedAtAction(nameof(GetClaimRewardByUserId), new { userId = createdUserReward.UserId, rewardId = createdUserReward.RewardId }, createdUserReward);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "USER")]
        [HttpDelete("{userId}/{rewardId}")]
        public async Task<IActionResult> DeleteUserReward(int userId, int rewardId)
        {
            if (await _userRewardService.DeleteUserRewardAsync(userId, rewardId))
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
