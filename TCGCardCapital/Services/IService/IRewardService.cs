using TCGCardCapital.DTOs;

namespace TCGCardCapital.Services.IService
{
    public interface IRewardService
    {
        Task<IEnumerable<RewardDTO>> GetRewardsAsync();
        Task<RewardDTO> GetRewardyByIdAsync(int id);
        Task<RewardDTO> CreateRewardAsync(RewardDTO rewardDTO);
        Task<bool> UpdateRewardAsync(int id, RewardDTO rewardDTO);
        Task<bool> DeleteRewardAsync(int id);
    }
}
