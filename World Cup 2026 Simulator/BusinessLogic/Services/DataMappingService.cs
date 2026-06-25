using World_Cup_2026_Simulator.BusinessLogic.Models;
using World_Cup_2026_Simulator.DataAccess.DTOs;

namespace World_Cup_2026_Simulator.BusinessLogic.Services
{
    public static class DataMappingService
    {
        public static Dictionary<string, ThirdPlaceMatrix> MapListToDictionary(List<ThirdPlaceMatrix> thirdPlaceMatrices)
        {
            return thirdPlaceMatrices.ToDictionary(m => m.CombinationKey);
        }

        public static TournamentState BuildTournamentState(TournamentRootDto tournamentRootDto)
        {
             var teams = BuildTeams(tournamentRootDto.Teams);

            var groups = BuildGroups(teams);

            var matches = BuildGroupStageMatches(groups);

            return new TournamentState(teams, groups, matches);
        }

        private static List<Team> BuildTeams(List<TeamDto> teamDtos)
        {
            var teams = teamDtos
                .Select(td => new Team(td.Name, td.Code, td.Group, CalculatePowerIndex(td.FifaPoints, td.SquadValueEurM)))
                .ToList();

            return teams;                
        }

        private static double CalculatePowerIndex(double fifaPoints, double squadValue)
        {
            const double fifaWeight = 0.80;
            const double squadValueWeight = 0.20;

            const double minFifa = 700;
            const double maxFifa = 2000;

            double normalizedFifa = (fifaPoints - minFifa) / (maxFifa - minFifa) * 100;
            normalizedFifa = Math.Clamp(normalizedFifa, 1.0, 100.0);

            double logSquadValue = Math.Log10(squadValue);

            double minLog = Math.Log10(5.0);
            double maxLog = Math.Log10(1500.0);

            double normalizedSquadValue = (logSquadValue - minLog) / (maxLog - minLog) * 100;
            normalizedSquadValue = Math.Clamp(normalizedSquadValue, 1.0, 100.0);

            double powerIndex = (normalizedFifa * fifaWeight) + (normalizedSquadValue * squadValueWeight);

            return Math.Round(powerIndex, 2);
        }

        private static List<Group> BuildGroups(List<Team> teams)
        {
            var groups = teams
                .GroupBy(t => t.GroupLetter)
                .Select(g => new Group
                {
                    Name = g.Key,
                    GroupStandings = g.Select(t => new GroupStanding(t)).ToList(),
                })
                .OrderBy(g => g.Name)
                .ToList();

            return groups;
        }

        private static List<Match> BuildGroupStageMatches(List<Group> groups)
        {
            var matches = groups.SelectMany(group =>
            {
                var teams = group.GroupStandings.Select(gs => gs.Team).ToList();

                return teams.SelectMany((homeTeam, index) => 
                    teams
                    .Skip(index + 1)
                    .Select(team => 
                        new Match(homeTeam, team, false))
                    .ToList());
                    
            })
            .ToList();

            return matches;
        }
    }
}
