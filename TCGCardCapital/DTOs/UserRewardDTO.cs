namespace TCGCardCapital.DTOs
{
    public class UserRewardDTO
    {
        public int? UserRewardId { get; set; }

        public int UserId { get; set; }

        public int RewardId { get; set; }

        public DateTime? ClaimedDate { get; set; }
    }
}
