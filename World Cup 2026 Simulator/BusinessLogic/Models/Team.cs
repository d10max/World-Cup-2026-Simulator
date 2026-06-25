using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World_Cup_2026_Simulator.Helpers;

namespace World_Cup_2026_Simulator.BusinessLogic.Models
{
    public class Team
    {
        public string Name { get; init; }

        public string Code { get; init; }

        public string GroupLetter { get; init; }

        public double PowerIndex { get; init; }

        public Team(string name, string code, string groupLetter, double powerIndex)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(groupLetter);

            ArgumentOutOfRangeException.ThrowIfNegative(powerIndex);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(powerIndex, 100);

            Name = name;
            Code = code;
            GroupLetter = groupLetter;
            PowerIndex = powerIndex;
        }
    }
}
