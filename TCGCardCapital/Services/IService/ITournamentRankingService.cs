using TCGCardCapital.DTOs;

namespace TCGCardCapital.Services.IService
{
    public interface ITournamentRankingService
    {
        Task<IEnumerable<TournamentRankingDTO>> GetRankingByTournamentIdAsync(int tournamentId);
        Task<bool> UpdateUserRankAsync(int tournamentId, int userId, int rank);

        Task<bool> DeleteRankingsByTournamentIdUserIdAsync(int tournamentId, int userId);
    }
}
