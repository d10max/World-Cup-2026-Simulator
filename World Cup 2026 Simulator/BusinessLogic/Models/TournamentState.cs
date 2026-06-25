using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World_Cup_2026_Simulator.BusinessLogic.Models
{
    public class TournamentState
    {
        public List<Team> Teams { get; set; }

        public List<Group> Groups { get; set; }

        public List<Match> GroupStageMatches { get; set; }

        public TournamentState(List<Team> teams, List<Group> groups, List<Match> matches)
        {
            ArgumentNullException.ThrowIfNull(teams);
            ArgumentNullException.ThrowIfNull(groups);
            ArgumentNullException.ThrowIfNull(matches);

            Teams = teams;
            Groups = groups;
            GroupStageMatches = matches;
        }
    }
}
