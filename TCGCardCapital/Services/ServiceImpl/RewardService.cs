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
            if (rewardDTO.Image != null)
            {
                // Generate a unique file name
                var fileName = Guid.NewGuid() + Path.GetExtension(rewardDTO.Image.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                var uniqueFilePath = Path.Combine(uploadsFolder, fileName);

                // Ensure the upload directory exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Save the file to disk
                using (var stream = new FileStream(uniqueFilePath, FileMode.Create))
                {
                    await rewardDTO.Image.CopyToAsync(stream);
                }

                // Save only the file name in the DTO
                rewardDTO.ImageUrl = fileName; // Save just the filename or a relative path
            }

            // Map DTO to Product entity
            var reward = _mapper.Map<Reward>(rewardDTO);
            _context.Rewards.Add(reward);
            await _context.SaveChangesAsync();

            // Return the mapped ProductDTO
            return _mapper.Map<RewardDTO>(reward);
        }

        public async Task<bool> UpdateRewardAsync(int id, RewardDTO rewardDTO)
        {
            var reward = await _context.Rewards.FindAsync(id);
            if (reward == null) return false;

            // Define the uploads folder path
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            // If a new image is provided
            if (rewardDTO.Image != null)
            {
                // If there's an existing image, delete it
                if (!string.IsNullOrEmpty(reward.ImageUrl))
                {
                    var oldImagePath = Path.Combine(uploadsFolder, reward.ImageUrl);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath); // Delete the old image
                    }
                }

                // Save the new image
                var fileName = Guid.NewGuid() + Path.GetExtension(rewardDTO.Image.FileName);
                var uniqueFilePath = Path.Combine(uploadsFolder, fileName);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var stream = new FileStream(uniqueFilePath, FileMode.Create))
                {
                    await rewardDTO.Image.CopyToAsync(stream);
                }

                // Update the reward's ImageUrl with the new file name
                rewardDTO.ImageUrl = fileName;
            }

            // Map updated fields from rewardDTO to reward entity
            reward.RewardName = rewardDTO.RewardName;
            reward.Description = rewardDTO.Description;
            reward.PointsRequired = rewardDTO.PointsRequired;
            reward.IsExtraReward = rewardDTO.IsExtraReward;
            reward.ImageUrl = rewardDTO.ImageUrl ?? reward.ImageUrl; // Only update if new image is provided

            _context.Rewards.Update(reward);
            await _context.SaveChangesAsync();

            return true;
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
