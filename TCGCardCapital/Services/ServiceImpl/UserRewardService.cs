using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCGCardCapital.DTOs;
using TCGCardCapital.Models;
using TCGCardCapital.Services.IService;

namespace TCGCardCapital.Services.ServiceImpl
{
    public class UserRewardService : IUserRewardService
    {
        private readonly TcgcardCapitalContext _context;
        private readonly IMapper _mapper;

        public UserRewardService(TcgcardCapitalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserRewardDTO>> GetClaimedRewardsByUserIdAsync(int userId)
        {
            var rewards = await _context.UserRewards
                .Where(u => u.UserId == userId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<UserRewardDTO>>(rewards);
        }
        public async Task<UserRewardDTO> GetClaimedRewardByUserIdAsync(int userId, int rewardId)
        {
            var reward = await _context.UserRewards
               .Where(u => u.UserId == userId && u.RewardId == rewardId)
               .FirstOrDefaultAsync();

            return _mapper.Map<UserRewardDTO>(reward);
        }

        public async Task<UserRewardDTO> CreateUserRewardAsync(UserRewardDTO userRewardDTO)
        {

            var userReward = new UserReward
            {
                UserId = userRewardDTO.UserId, 
                RewardId = userRewardDTO.RewardId,
                ClaimedDate = DateTime.UtcNow 
            };

            _context.UserRewards.Add(userReward); 
            await _context.SaveChangesAsync(); 

            return _mapper.Map<UserRewardDTO>(userReward); 
        }

        public async Task<bool> DeleteUserRewardAsync(int userId, int rewardId)
        {
            var userReward = await _context.UserRewards
                .FirstOrDefaultAsync(c => c.UserId == userId && c.RewardId == rewardId);

            if (userReward == null) return false;

            _context.UserRewards.Remove(userReward);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
