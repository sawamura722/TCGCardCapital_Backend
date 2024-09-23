using TCGCardCapital.DTOs;

namespace TCGCardCapital.Services.IService
{
    public interface ITournamentService
    {
        Task<IEnumerable<TournamentDTO>> GetTournamentsAsync();
        Task<TournamentDTO> GetTournamentByIdAsync(int id);
        Task<TournamentDTO> CreateTournamentAsync(TournamentDTO tournamentDTO);
        Task<bool> UpdateTournamentAsync(int id, TournamentDTO tournamentDTO);
        Task<bool> DeleteTournamentAsync(int id);
    }
}
