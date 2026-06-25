using System.Net.WebSockets;
using World_Cup_2026_Simulator.BusinessLogic.Interfaces;
using World_Cup_2026_Simulator.BusinessLogic.Models;
using World_Cup_2026_Simulator.BusinessLogic.Services;

namespace World_Cup_2026_Simulator.BusinessLogic.Simulation
{
    public class TournamentSimulator
    {
        private readonly ITournamentRepository tournamentRepository;

        private readonly ITournamentUI tournamentUI;

        private readonly MatchSimulator matchSimulator;

        public void RunSimulation()
        {
            int mode = tournamentUI.GetSimulationMode();

            if (mode == 1)
            {
                RunTournamentSimulation();
            }
            else
            {
                int iterations = tournamentUI.GetIterationsNumber();

                RunMultipleTournamentSimulations(iterations);
            }
        }

        public TournamentSimulator(ITournamentRepository tournamentRepository, ITournamentUI tournamentUI)
        {
            ArgumentNullException.ThrowIfNull(tournamentRepository);
            ArgumentNullException.ThrowIfNull(tournamentUI);

            this.tournamentRepository = tournamentRepository;
            this.tournamentUI = tournamentUI;
            this.matchSimulator = new MatchSimulator();
        }

        public void RunMultipleTournamentSimulations(int iterations)
        {
            var statsTracker = new Dictionary<string, TournamentMonteCarloStats>();

            var thirdPlacesMatrices = tournamentRepository.GetThirdPlaceMatrices();
            var matricesDict = DataMappingService.MapListToDictionary(thirdPlacesMatrices);

            for (int i = 1; i <= iterations; i++)
            {
                var rawData = tournamentRepository.GetTournamentData();
                var tournamentState = DataMappingService.BuildTournamentState(rawData);

                if (i == 1)
                {
                    var allTeams = tournamentState.Teams;
                    foreach (var team in allTeams)
                    {
                        statsTracker[team.Name] = new TournamentMonteCarloStats { TeamName = team .Name };
                    }
                }

                SimulateGroupStage(tournamentState.Groups, tournamentState.GroupStageMatches);

                var roundOf32Matches = GenerateRoundOf32Matches(tournamentState.Groups, matricesDict);
                SimulateRound(roundOf32Matches);

                var roundOf16Matches = GetFinalRoundsMatches(roundOf32Matches);
                SimulateRound(roundOf16Matches);

                var quarterFinals = GetFinalRoundsMatches(roundOf16Matches);
                SimulateRound(quarterFinals);

                var semiFinals = GetFinalRoundsMatches(quarterFinals);
                SimulateRound(semiFinals);

                var final = GetFinalRoundsMatches(semiFinals);
                SimulateRound(final);

                RecordStageReached(statsTracker, roundOf32Matches, 0);
                RecordStageReached(statsTracker, roundOf16Matches, 1);
                RecordStageReached(statsTracker, quarterFinals, 2);    
                RecordStageReached(statsTracker, semiFinals, 3);       
                RecordStageReached(statsTracker, final, 4);

                var champion = final[0].GetKnockoutMatchWinner();
                statsTracker[champion.Name].StagesReached[5]++;
            }

            tournamentUI.DisplayMultipleTournamentSimulationResults(statsTracker, iterations);
        }

        public void RunTournamentSimulation()
        {
            var rawData = tournamentRepository.GetTournamentData();
            var tournamentState = DataMappingService.BuildTournamentState(rawData);
            
            SimulateGroupStage(tournamentState.Groups, tournamentState.GroupStageMatches);

            tournamentUI.DisplayGroups(tournamentState.Groups);

            var thirdPlaceStandings = tournamentState.Groups
                .Select(g => g.GroupStandings[2])
                .OrderByDescending(gs => gs.Points)
                .ThenByDescending(gs => gs.GoalsDifference)
                .ThenByDescending(gs => gs.GoalsScored)
                .ThenByDescending(gs => gs.Team.PowerIndex)
                .ToList();

            tournamentUI.DisplayThirdPlacesStandings(thirdPlaceStandings);

            var thirdPlacesMatrices = tournamentRepository.GetThirdPlaceMatrices();
            var matricesDict = DataMappingService.MapListToDictionary(thirdPlacesMatrices);

            var roundOf32Matches = GenerateRoundOf32Matches(tournamentState.Groups, matricesDict);
            SimulateRound(roundOf32Matches);
            tournamentUI.DisplayRoundResults("1/16 фіналу", roundOf32Matches);

            var roundOf16Matches = GetFinalRoundsMatches(roundOf32Matches);
            SimulateRound(roundOf16Matches);
            tournamentUI.DisplayRoundResults("1/8 фіналу", roundOf16Matches);


            var quarterFinals = GetFinalRoundsMatches(roundOf16Matches);
            SimulateRound(quarterFinals);
            tournamentUI.DisplayRoundResults("Чвертьфінали", quarterFinals);


            var semiFinals = GetFinalRoundsMatches(quarterFinals);
            SimulateRound(semiFinals);
            tournamentUI.DisplayRoundResults("Півфінали", semiFinals);


            var thirdPlaceMatch = new Match(semiFinals[0].GetKnockoutMatchLoser(), semiFinals[1].GetKnockoutMatchLoser(), true);
            SimulateRound(new List<Match>() { thirdPlaceMatch });
            tournamentUI.DisplayRoundResults("Матч за третє місце", new List<Match>() { thirdPlaceMatch });


            var final = GetFinalRoundsMatches(semiFinals);
            SimulateRound(final);
            tournamentUI.DisplayRoundResults("Фінал", final);
        }

        private void RecordStageReached(Dictionary<string, TournamentMonteCarloStats> statsTracker, List<Match> matches, int stageIndex)
        {
            foreach (var match in matches)
            {
                statsTracker[match.HomeTeam.Name].StagesReached[stageIndex]++;
                statsTracker[match.AwayTeam.Name].StagesReached[stageIndex]++;
            }
        }

        private List<Match> GetFinalRoundsMatches(List<Match> previousRoundMatches)
        {
            var nextRoundMatches = new List<Match>();
            for (int i = 0; i <  previousRoundMatches.Count; i += 2)
            {
                var winner1 = previousRoundMatches[i].GetKnockoutMatchWinner();
                var winner2 = previousRoundMatches[i + 1].GetKnockoutMatchWinner();

                nextRoundMatches.Add(new Match(winner1, winner2, true));
            }

            return nextRoundMatches;
        }

        private void SimulateRound(List<Match> matches)
        {
            foreach (var match in matches)
            {
                matchSimulator.SimulateMatch(match);
            }
        }

        private List<Match> GenerateRoundOf32Matches(List<Group> groups, Dictionary<string, ThirdPlaceMatrix> matricesDict)
        {
            var firstPlaces = groups
                .Select(g => g.GroupStandings[0].Team)
                .ToList();

            var secondPlaces = groups
                .Select(g => g.GroupStandings[1].Team)
                .ToList();

            var thirdPlaces = groups
                .Select(g => g.GroupStandings[2])
                .OrderByDescending(gs => gs.Points)
                .ThenByDescending(gs => gs.GoalsDifference)
                .ThenByDescending(gs => gs.GoalsScored)
                .ThenByDescending(gs => gs.Team.PowerIndex)
                .Take(8)
                .Select(gs => gs.Team)
                .ToList();

            string combinationKey = new string(thirdPlaces
                .Select(t => t.GroupLetter[0])
                .OrderBy(n => n)
                .ToArray());

            var thirdPlacesMatrix = matricesDict[combinationKey];

            var roundOf32 = new List<Match>();

            roundOf32.Add(new Match(GetTeam(firstPlaces, "E"), GetThirdPlaceTeam(thirdPlaces, thirdPlacesMatrix.Slot1E), true));
            roundOf32.Add(new Match(GetTeam(firstPlaces, "I"), GetThirdPlaceTeam(thirdPlaces, thirdPlacesMatrix.Slot1I), true));
            roundOf32.Add(new Match(GetTeam(secondPlaces, "A"), GetTeam(secondPlaces, "B"), true));
            roundOf32.Add(new Match(GetTeam(firstPlaces, "F"), GetTeam(secondPlaces, "C"), true));

            roundOf32.Add(new Match(GetTeam(secondPlaces, "K"), GetTeam(secondPlaces, "L"), true));
            roundOf32.Add(new Match(GetTeam(firstPlaces, "H"), GetTeam(secondPlaces, "J"), true));
            roundOf32.Add(new Match(GetTeam(firstPlaces, "D"), GetThirdPlaceTeam(thirdPlaces, thirdPlacesMatrix.Slot1D), true));
            roundOf32.Add(new Match(GetTeam(firstPlaces, "G"), GetThirdPlaceTeam(thirdPlaces, thirdPlacesMatrix.Slot1G), true));

            roundOf32.Add(new Match(GetTeam(firstPlaces, "C"), GetTeam(secondPlaces, "F"), true));
            roundOf32.Add(new Match(GetTeam(secondPlaces, "E"), GetTeam(secondPlaces, "I"), true));
            roundOf32.Add(new Match(GetTeam(firstPlaces, "A"), GetThirdPlaceTeam(thirdPlaces, thirdPlacesMatrix.Slot1A), true));
            roundOf32.Add(new Match(GetTeam(firstPlaces, "L"), GetThirdPlaceTeam(thirdPlaces, thirdPlacesMatrix.Slot1L), true));

            roundOf32.Add(new Match(GetTeam(firstPlaces, "J"), GetTeam(secondPlaces, "H"), true));
            roundOf32.Add(new Match(GetTeam(secondPlaces, "D"), GetTeam(secondPlaces, "G"), true));
            roundOf32.Add(new Match(GetTeam(firstPlaces, "B"), GetThirdPlaceTeam(thirdPlaces, thirdPlacesMatrix.Slot1B), true));
            roundOf32.Add(new Match(GetTeam(firstPlaces, "K"), GetThirdPlaceTeam(thirdPlaces, thirdPlacesMatrix.Slot1K), true));

            return roundOf32;
        }

        private void SimulateGroupStage(List<Group> groups, List<Match> matches)
        {
            var groupStandings = groups.SelectMany(g => g.GroupStandings).ToList();

            foreach (var match in matches)
            {
                matchSimulator.SimulateMatch(match);
                UpdateGroupStandings(match, groupStandings);
            }

            SortGroupStandings(groups);
        }

        private Team GetTeam(List<Team> teams, string groupLetter)
        {
            return teams.First(t => t.GroupLetter == groupLetter);
        }

        private Team GetThirdPlaceTeam(List<Team> thirdPlaces, string slotCode)
        {
            string targetGroup = slotCode.Replace("3", "").Trim();

            return thirdPlaces.First(t => t.GroupLetter == targetGroup);
        }

        private void UpdateGroupStandings(Match match, List<GroupStanding> groupStandings)
        {
            var homeTeamStanding = groupStandings.First(gs => gs.Team == match.HomeTeam);
            var awayTeamStanding = groupStandings.First(gs => gs.Team == match.AwayTeam);

            homeTeamStanding.MatchesPlayed++;
            awayTeamStanding.MatchesPlayed++;

            homeTeamStanding.GoalsScored += match.HomeGoals;
            homeTeamStanding.GoalsConceded += match.AwayGoals;

            awayTeamStanding.GoalsScored += match.AwayGoals;
            awayTeamStanding.GoalsConceded += match.HomeGoals;

            if (match.HomeGoals > match.AwayGoals)
            {
                homeTeamStanding.Won++;
                awayTeamStanding.Lost++;
            }
            else if (match.AwayGoals > match.HomeGoals)
            {
                awayTeamStanding.Won++;
                homeTeamStanding.Lost++;
            }
            else
            {
                homeTeamStanding.Draw++;
                awayTeamStanding.Draw++;
            }
        }

        private void SortGroupStandings(List<Group> groups)
        {
            foreach (var group in groups)
            {
                group.GroupStandings = group.GroupStandings
                    .OrderByDescending(g => g.Points)
                    .ThenByDescending(g => g.GoalsDifference)
                    .ThenByDescending(g => g.GoalsScored)
                    .ThenByDescending(g => g.Team.PowerIndex)
                    .ToList();
            }
        }
    }
}
