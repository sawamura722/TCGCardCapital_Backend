using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCGCardCapital.DTOs;
using TCGCardCapital.Models;
using TCGCardCapital.Services.IService;

namespace TCGCardCapital.Services.ServiceImpl
{
    public class TournamentRegistrationService : ITournamentRegistrationService
    {
        private readonly TcgcardCapitalContext _context;
        private readonly IMapper _mapper;

        public TournamentRegistrationService(TcgcardCapitalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TournamentRegistrationDTO>> GetAllTournamentUsersAsync()
        {
            var users = await _context.Rankings
                .Include(r => r.Tournament)
                .Include(r => r.User)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TournamentRegistrationDTO>>(users);
        }

        public async Task<IEnumerable<TournamentRegistrationDTO>> GetTournamentUsersByUserIdAsync(int userId)
        {
            var users = await _context.Rankings
                .Include(r => r.Tournament)
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TournamentRegistrationDTO>>(users);
        }

        public async Task<bool> RegisterUserForTournamentAsync(TournamentRegistrationDTO registrationDTO)
        {
            // Check if the user is already registered for the tournament
            var existingRegistration = await _context.Rankings
                .FirstOrDefaultAsync(r => r.TournamentId == registrationDTO.TournamentId && r.UserId == registrationDTO.UserId);

            if (existingRegistration == null)
            {
                // Create a new registration
                var newRegistration = new Ranking
                {
                    TournamentId = registrationDTO.TournamentId,
                    UserId = registrationDTO.UserId,
                    Rank = null // No rank when registering
                };

                _context.Rankings.Add(newRegistration);
                await _context.SaveChangesAsync();
                return true;
            }

            return false; // User already registered
        }
        public async Task<bool> DeleteTournamentRegistrationByUserIdAsync(int userId) 
        {
            var registrations = await _context.Rankings
                .Where(r => r.UserId == userId)
                .ToListAsync();

            if (registrations.Any())
            {
                _context.Rankings.RemoveRange(registrations);
                await _context.SaveChangesAsync();
                return true;
            }

            return false; // No registrations found for the user
        }
    }
}
