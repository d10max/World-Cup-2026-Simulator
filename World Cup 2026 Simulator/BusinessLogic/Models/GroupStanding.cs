using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World_Cup_2026_Simulator.BusinessLogic.Models
{
    public class GroupStanding
    {
        public Team Team { get; init; }

        public int MatchesPlayed { get; set; }

        public int Won { get; set; }

        public int Draw { get; set; }

        public int Lost { get; set; }

        public int Points => (Won * 3) + (Draw * 1);

        public int GoalsScored { get; set; }

        public int GoalsConceded { get; set; }

        public int GoalsDifference => GoalsScored - GoalsConceded;

        public GroupStanding(Team team)
        {
            ArgumentNullException.ThrowIfNull(team);

            Team = team;
        }

        public GroupStanding() { }
    }
}
