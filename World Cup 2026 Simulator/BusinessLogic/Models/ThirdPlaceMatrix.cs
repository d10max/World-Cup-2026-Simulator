using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace World_Cup_2026_Simulator.BusinessLogic.Models
{
    public class ThirdPlaceMatrix
    {
        [JsonPropertyName("combinationKey")]
        public string CombinationKey { get; set; }

        [JsonPropertyName("groupsQualified")]
        public string[] GroupsQualified { get; set; }

        [JsonPropertyName("slot_1A")]
        public string Slot1A { get; set; }

        [JsonPropertyName("slot_1B")]
        public string Slot1B { get; set; }

        [JsonPropertyName("slot_1D")]
        public string Slot1D { get; set; }

        [JsonPropertyName("slot_1E")]
        public string Slot1E { get; set; }

        [JsonPropertyName("slot_1G")]
        public string Slot1G { get; set; }

        [JsonPropertyName("slot_1I")]
        public string Slot1I { get; set; }

        [JsonPropertyName("slot_1K")]
        public string Slot1K { get; set; }

        [JsonPropertyName("slot_1L")]
        public string Slot1L { get; set; }
    }
}
