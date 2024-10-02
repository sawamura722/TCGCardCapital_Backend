using TCGCardCapital.DTOs;

namespace TCGCardCapital.Services.IService
{
    public interface IUserRewardService
    {
        Task<IEnumerable<UserRewardDTO>> GetClaimedRewardsByUserIdAsync(int userId);
        Task<UserRewardDTO> GetClaimedRewardByUserIdAsync(int userId, int rewardId);
        Task<UserRewardDTO> CreateUserRewardAsync(UserRewardDTO userRewardDTO);
        Task<bool> DeleteUserRewardAsync(int userId, int rewardId);
    }
}
