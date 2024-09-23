using System;
using System.Collections.Generic;

namespace TCGCardCapital.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int RoleId { get; set; }

    public bool SubscriptionStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int Point { get; set; }

    public string? Location { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Ranking> Rankings { get; set; } = new List<Ranking>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<UserReward> UserRewards { get; set; } = new List<UserReward>();
}
