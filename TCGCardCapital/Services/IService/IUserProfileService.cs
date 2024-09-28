using TCGCardCapital.DTOs;

namespace TCGCardCapital.Services.IService
{
    public interface IUserProfileService
    {
        Task<UserProfileDTO> GetUserProfileByUserIdAsync(int userId);
        Task<bool> UpdateUserProfileAsync(int userId, UserProfileDTO userProfileDTO);
    }
}
