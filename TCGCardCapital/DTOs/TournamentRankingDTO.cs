namespace TCGCardCapital.DTOs
{
    public class TournamentRankingDTO
    {
        public int TournamentId { get; set; }
        public int UserId { get; set; }
        public int? Rank { get; set; }
        public string UserName { get; set; }
    }
}
