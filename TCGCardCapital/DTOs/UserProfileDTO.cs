namespace TCGCardCapital.DTOs
{
    public class UserProfileDTO
    {
        public int? UserId { get; set; }

        public string Username { get; set; } = null!;

        public string? Email { get; set; } = null!;

        public bool SubscriptionStatus { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? Point { get; set; }

        public string? Location { get; set; }
    }
}
