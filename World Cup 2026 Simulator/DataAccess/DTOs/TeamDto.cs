using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace World_Cup_2026_Simulator.DataAccess.DTOs
{
    public class TeamDto
    {
        [JsonPropertyName("team")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("group")]
        public string Group { get; set; } = string.Empty;

        [JsonPropertyName("fifa_points")]
        public double FifaPoints { get; set; }

        [JsonPropertyName("squad_value_eur_m")]
        public double SquadValueEurM { get; set; }
    }
}
