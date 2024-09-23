using TCGCardCapital.DTOs;

namespace TCGCardCapital.Services.IService
{
    public interface ITournamentRegistrationService
    {
        Task<IEnumerable<TournamentRegistrationDTO>> GetAllTournamentUsersAsync();
        Task<IEnumerable<TournamentRegistrationDTO>> GetTournamentUsersByUserIdAsync(int userId);
        Task<bool> RegisterUserForTournamentAsync(TournamentRegistrationDTO registrationDTO); 
        Task<bool> DeleteTournamentRegistrationByUserIdAsync(int userId);
    }
}
