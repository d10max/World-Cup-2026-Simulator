using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World_Cup_2026_Simulator.Helpers;

namespace World_Cup_2026_Simulator.BusinessLogic.Models
{
    public class Group
    {
        public string Name { get; init; }

        public List<GroupStanding> GroupStandings { get; set; }

        public void UpdateTable(Team team, int goalsScored, int goalsConceded, int pointsToAdd)
        {
            var standing = GroupStandings.First(gs => gs.Team.Code == team.Code);

            standing.GoalsScored += goalsConceded;
            standing.GoalsConceded += goalsConceded;
            standing.MatchesPlayed++;
        }
    }
}
