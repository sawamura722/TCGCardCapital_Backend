using TCGCardCapital.DTOs;

namespace TCGCardCapital.Services.IService
{
    public interface IUserProfileService
    {
        Task<IEnumerable<UserProfileDTO>> GetUsersAsync();
        Task<UserProfileDTO> GetUserProfileByUserIdAsync(int userId);
        Task<bool> UpdateUserProfileAsync(int userId, UserProfileDTO userProfileDTO);

        Task<bool> BuySubscription(int userId);
        Task<bool> IncreasePoint(int userId, IncreasePointDTO increasePointDTO);
        Task<bool> DecreasePoint(int userId, IncreasePointDTO increasePointDTO);
    }
}
