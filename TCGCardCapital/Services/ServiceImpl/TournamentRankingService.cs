using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCGCardCapital.DTOs;
using TCGCardCapital.Models;
using TCGCardCapital.Services.IService;

namespace TCGCardCapital.Services.ServiceImpl
{
    public class TournamentRankingService : ITournamentRankingService
    {
        private readonly TcgcardCapitalContext _context;
        private readonly IMapper _mapper;

        public TournamentRankingService(TcgcardCapitalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TournamentRankingDTO>> GetRankingByTournamentIdAsync(int tournamentId)
        {
            var rankings = await _context.Rankings
                .Include(r => r.User)
                .Where(r => r.TournamentId == tournamentId)
                .Select(r => new TournamentRankingDTO
                {
                    TournamentId = r.TournamentId,
                    UserId = r.UserId,
                    Rank = r.Rank,
                    // Assuming UserName is a property of User entity
                    UserName = r.User.Username
                })
                .ToListAsync();

            return rankings;
        }

        public async Task<bool> UpdateUserRankAsync(int tournamentId, int userId, int rank)
        {
            var ranking = await _context.Rankings
                .FirstOrDefaultAsync(r => r.TournamentId == tournamentId && r.UserId == userId);

            if (ranking != null)
            {
                ranking.Rank = rank;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteRankingsByTournamentIdUserIdAsync(int tournamentId, int userId)
        {
            var rankings = await _context.Rankings
                .Where(t => t.TournamentId == tournamentId && t.UserId == userId)
                .ToListAsync();

            if (rankings.Any())
            {
                _context.Rankings.RemoveRange(rankings);
                await _context.SaveChangesAsync();
                return true;
            }

            return false; // No rankings found
        }
    }
}
