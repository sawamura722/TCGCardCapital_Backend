using System;
using System.Collections.Generic;

namespace TCGCardCapital.Models;

public partial class UserReward
{
    public int UserRewardId { get; set; }

    public int UserId { get; set; }

    public int RewardId { get; set; }

    public DateTime ClaimedDate { get; set; }

    public virtual Reward Reward { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
