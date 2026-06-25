using World_Cup_2026_Simulator.BusinessLogic.Simulation;
using World_Cup_2026_Simulator.DataAccess.Repositories;
using World_Cup_2026_Simulator.Presentation.UI;

namespace World_Cup_2026_Simulator
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var consoleUI = new ConsoleUIService();
            var repository = new TournamentRepository();

            var tournamentSim = new TournamentSimulator(repository, consoleUI);
            tournamentSim.RunSimulation();
        }
    }
}
