using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World_Cup_2026_Simulator.BusinessLogic.Models
{
    public class TournamentMonteCarloStats
    {
        public string TeamName { get; set; }

        // Масив для зберігання етапів (індекси: 0 - Група, 1 - 1/16, 2 - 1/8, 3 - 1/4, 4 - 1/2, 5 - Фіналіст, 6 - Чемпіон)
        public int[] StagesReached { get; set; } = new int[6];
    }
}
