using World_Cup_2026_Simulator.BusinessLogic.Models;

namespace World_Cup_2026_Simulator.BusinessLogic.Interfaces
{
    public interface ITournamentUI
    {
        void DisplayMessage(string message);

        void DisplayGreetings();

        void DisplayGroups(List<Group> groups);

        void DisplayThirdPlacesStandings(List<GroupStanding> thirdPlacesStandings);

        void DisplayRoundResults(string roundName, List<Match> matches);

        void DisplayMultipleTournamentSimulationResults(Dictionary<string, TournamentMonteCarloStats> statsTracker, int iterations);

        int GetSimulationMode();

        int GetIterationsNumber();
    }
}