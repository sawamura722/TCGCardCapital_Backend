using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TCGCardCapital.DTOs;
using TCGCardCapital.Models;
using TCGCardCapital.Services.IService;


namespace TCGCardCapital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RewardsController : ControllerBase
    {
        private readonly IRewardService _rewardService;

        public RewardsController(IRewardService rewardService)
        {
            _rewardService = rewardService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RewardDTO>>> GetRewards()
        {
            var rewards = await _rewardService.GetRewardsAsync();
            return Ok(rewards);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RewardDTO>> GetReward(int id)
        {
            var reward = await _rewardService.GetRewardyByIdAsync(id);

            if (reward == null)
            {
                return NotFound();
            }

            return Ok(reward);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<ActionResult<RewardDTO>> PostReward([FromForm] RewardDTO rewardDTO)
        {
            var createdReward = await _rewardService.CreateRewardAsync(rewardDTO);
            return CreatedAtAction(nameof(GetReward), new { id = createdReward.RewardId }, createdReward);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReward(int id,[FromForm] RewardDTO rewardDTO)
        {
            if (await _rewardService.UpdateRewardAsync(id, rewardDTO))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReward(int id)
        {
            if (await _rewardService.DeleteRewardAsync(id))
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
