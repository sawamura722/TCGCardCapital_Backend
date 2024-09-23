using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCGCardCapital.DTOs;
using TCGCardCapital.Models;
using TCGCardCapital.Services.IService;

namespace TCGCardCapital.Services.ServiceImpl
{
    public class RewardService : IRewardService
    {
        private readonly TcgcardCapitalContext _context;
        private readonly IMapper _mapper;

        public RewardService(TcgcardCapitalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RewardDTO>> GetRewardsAsync()
        {
            var rewards = await _context.Rewards.ToListAsync();
            return _mapper.Map<IEnumerable<RewardDTO>>(rewards);
        }

        public async Task<RewardDTO> GetRewardyByIdAsync(int id)
        {
            var reward = await _context.Rewards.FindAsync(id);
            if (reward == null) return null;

            return _mapper.Map<RewardDTO>(reward);
        }

        public async Task<RewardDTO> CreateRewardAsync(RewardDTO rewardDTO)
        {
            var reward = _mapper.Map<Reward>(rewardDTO);
            reward.CreatedAt = DateTime.Now;

            _context.Rewards.Add(reward);
            await _context.SaveChangesAsync();

            return _mapper.Map<RewardDTO>(reward);
        }

        public async Task<bool> UpdateRewardAsync(int id, RewardDTO rewardDTO)
        {
            var existingReward = await _context.Rewards.FindAsync(id);

            if (existingReward == null)
                return false;

            _mapper.Map(rewardDTO, existingReward);

            existingReward.UpdatedAt = DateTime.Now;

            _context.Entry(existingReward).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Rewards.Any(r => r.RewardId == id))
                    return false;

                throw;
            }
        }


        public async Task<bool> DeleteRewardAsync(int id)
        {
            var reward = await _context.Rewards.FindAsync(id);
            if(reward == null) return false;

            _context.Rewards.Remove(reward);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
