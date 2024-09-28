namespace TCGCardCapital.DTOs
{
    public class RewardDTO
    {
        public int? RewardId { get; set; }

        public string RewardName { get; set; } = null!;

        public string? Description { get; set; }

        public int PointsRequired { get; set; }

        public bool? IsExtraReward { get; set; }
    }
}
