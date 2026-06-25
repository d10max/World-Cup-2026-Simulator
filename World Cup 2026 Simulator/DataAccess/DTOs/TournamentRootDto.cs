using System.Text.Json.Serialization;

namespace World_Cup_2026_Simulator.DataAccess.DTOs
{
    public class TournamentRootDto
    {
        [JsonPropertyName("teams")]
        public List<TeamDto> Teams { get; set; } = default!;
    }
}
