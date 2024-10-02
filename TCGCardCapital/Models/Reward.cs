using System;
using System.Collections.Generic;

namespace TCGCardCapital.Models;

public partial class Reward
{
    public int RewardId { get; set; }

    public string RewardName { get; set; } = null!;

    public string? Description { get; set; }

    public int PointsRequired { get; set; }

    public bool? IsExtraReward { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<UserReward> UserRewards { get; set; } = new List<UserReward>();
}
