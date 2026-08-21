namespace SportsLeague.Infrastructure.Models
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int TeamId { get; set; }
    }
}
