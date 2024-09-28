using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCGCardCapital.DTOs;
using TCGCardCapital.Models;
using TCGCardCapital.Services.IService;

namespace TCGCardCapital.Services.ServiceImpl
{
    public class TournamentService : ITournamentService
    {
        private readonly TcgcardCapitalContext _context;
        private readonly IMapper _mapper;

        public TournamentService(TcgcardCapitalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TournamentDTO>> GetTournamentsAsync()
        {
            var tournaments = await _context.Tournaments.ToListAsync();
            return _mapper.Map<IEnumerable<TournamentDTO>>(tournaments);
        }

        public async Task<TournamentDTO> GetTournamentByIdAsync(int id)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null) return null;

            return _mapper.Map<TournamentDTO>(tournament);
        }

        public async Task<TournamentDTO> CreateTournamentAsync(TournamentDTO tournamentDTO)
        {
            var tournament = _mapper.Map<Tournament>(tournamentDTO);
            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();
            return _mapper.Map<TournamentDTO>(tournament);
        }

        public async Task<bool> UpdateTournamentAsync(int id, TournamentDTO tournamentDTO)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            if(tournament == null) return false;

            tournament.Name = tournamentDTO.Name;
            tournament.Description = tournamentDTO.Description;
            tournament.Date = tournamentDTO.Date;
            tournament.Location = tournamentDTO.Location;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tournaments.Any(t => t.TournamentId == id))
                    return false;
                throw;
            }
        }

        public async Task<bool> DeleteTournamentAsync(int id)
        {
            var ranking = await _context.Rankings
                .Where(t => t.TournamentId == id)
                .ToListAsync();

            if (ranking.Any())
            {
                _context.Rankings.RemoveRange(ranking);
                await _context.SaveChangesAsync();
            }

            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null) return false;

            _context.Tournaments.Remove(tournament);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
