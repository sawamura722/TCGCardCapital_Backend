namespace TCGCardCapital.DTOs
{
    public class TournamentDTO
    {
        public int TournamentId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime Date { get; set; }

        public string Location { get; set; } = null!;
    }
}
