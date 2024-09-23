using System;
using System.Collections.Generic;

namespace TCGCardCapital.Models;

public partial class Ranking
{
    public int TournamentId { get; set; }

    public int UserId { get; set; }

    public int? Rank { get; set; }

    public virtual Tournament Tournament { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
