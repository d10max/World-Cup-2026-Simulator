using World_Cup_2026_Simulator.BusinessLogic.Interfaces;
using World_Cup_2026_Simulator.BusinessLogic.Models;

namespace World_Cup_2026_Simulator.Presentation.UI
{
    public class ConsoleUIService : ITournamentUI
    {
        public void DisplayGreetings()
        {
            throw new NotImplementedException();
        }

        public void DisplayGroups(List<Group> groups)
        {
            foreach (var group in groups)
            {
                Console.WriteLine("=============================================================");
                Console.WriteLine($"                          ГРУПА {group.Name}                          ");
                Console.WriteLine("=============================================================");

                Console.WriteLine(" # | Команда            | Pld | W | D | L | GF | GA |  GD | Pts");
                Console.WriteLine("-------------------------------------------------------------");

                int position = 1;
                foreach (var standing in group.GroupStandings)
                {
                    string gd = standing.GoalsDifference.ToString("+0;-#;0");

                    Console.WriteLine(
                        $" {position} | " +
                        $"{standing.Team.Name,-18} | " +
                        $"{standing.MatchesPlayed,3} | " +
                        $"{standing.Won,1} | " +
                        $"{standing.Draw,1} | " +
                        $"{standing.Lost,1} | " +
                        $"{standing.GoalsScored,2} | " +
                        $"{standing.GoalsConceded,2} | " +
                        $"{gd,3} | " +
                        $"{standing.Points,3}");

                    position++;
                }

                Console.WriteLine("=============================================================\n");
            }
        }

        public void DisplayThirdPlacesStandings(List<GroupStanding> thirdPlacesStandings)
        {
            Console.WriteLine("===================================================================");
            Console.WriteLine("              РЕЙТИНГ КОМАНД, ЩО ПОСІЛИ ТРЕТІ МІСЦЯ                ");
            Console.WriteLine("===================================================================");

            Console.WriteLine(" # | Команда            | Grp | Pld | W | D | L | GS | GA |  GD | Pts");
            Console.WriteLine("-------------------------------------------------------------------");

            int position = 1;
            foreach (var standing in thirdPlacesStandings)
            {
                string gd = standing.GoalsDifference.ToString("+0;-#;0");

                Console.WriteLine(
                    $" {position,1} | " +
                    $"{standing.Team.Name,-18} | " +
                    $"  {standing.Team.GroupLetter,1} | " +
                    $"{standing.MatchesPlayed,3} | " +
                    $"{standing.Won,1} | " +
                    $"{standing.Draw,1} | " +
                    $"{standing.Lost,1} | " +
                    $"{standing.GoalsScored,2} | " +
                    $"{standing.GoalsConceded,2} | " +
                    $"{gd,3} | " +
                    $"{standing.Points,3}");

                if (position == 8 && thirdPlacesStandings.Count > 8)
                {
                    Console.WriteLine("-------------------------------------------------------------------");
                    Console.WriteLine("                          ВИБУВАЮТЬ З ТУРНІРУ                      ");
                    Console.WriteLine("-------------------------------------------------------------------");
                }

                position++;
            }

            Console.WriteLine("===================================================================\n");
        }

        public void DisplayRoundResults(string roundName, List<Match> matches)
        {
            Console.WriteLine("\n=================================================================");

            int totalWidth = 65;
            int spaces = (totalWidth - roundName.Length) / 2;
            Console.WriteLine($"{new string(' ', Math.Max(0, spaces))}{roundName.ToUpper()}");

            Console.WriteLine("=================================================================");

            foreach (var match in matches)
            {
                string scoreStr = $"{match.HomeGoals} : {match.AwayGoals}";

                string penaltyStr = "";
                if (match.HomeGoals == match.AwayGoals)
                {
                    penaltyStr = $" (Пен: {match.HomePenalties}-{match.AwayPenalties})";
                }

                string homeName = match.HomeTeam.Name;
                string awayName = match.AwayTeam.Name;

                Team winner = match.GetKnockoutMatchWinner(); 
                if (winner == match.HomeTeam) homeName = $"* {homeName}";
                if (winner == match.AwayTeam) awayName = $"{awayName} *";

                Console.WriteLine($"{homeName,20}   {scoreStr,5}   {awayName,-20} {penaltyStr}");
            }

            Console.WriteLine("=================================================================\n");
        }

        public void DisplayMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void DisplayMultipleTournamentSimulationResults(Dictionary<string, TournamentMonteCarloStats> statsTracker, int iterations)
        {
            Console.WriteLine("\n=========================================================================================================");
            Console.WriteLine("                              ПРОГНОЗ ТУРНІРУ (ДОСЯГНЕННЯ СТАДІЙ)                          ");
            Console.WriteLine("=========================================================================================================");
            Console.WriteLine(" Команда         | Вихід в 1/16  | Вихід в 1/8   | Вихід в 1/4   | Півфінал      | Фінал         | ЧЕМПІОН");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------");

            var sortedStats = statsTracker.Values
                .OrderByDescending(s => s.StagesReached[5])
                .ThenByDescending(s => s.StagesReached[4])
                .ToList();

            foreach (var stat in sortedStats)
            {
                string FormatCell(int count)
                {
                    if (count == 0) return "      -      ";
                    double perc = (double)count / iterations * 100;
                    return $"{count,4} / {perc,4:F1}%";
                }

                Console.WriteLine(
            $" {stat.TeamName,-15} |" +
            $" {FormatCell(stat.StagesReached[0])} |" + // 1/16
            $" {FormatCell(stat.StagesReached[1])} |" + // 1/8
            $" {FormatCell(stat.StagesReached[2])} |" + // 1/4
            $" {FormatCell(stat.StagesReached[3])} |" + // 1/2
            $" {FormatCell(stat.StagesReached[4])} |" + // Фінал
            $" {FormatCell(stat.StagesReached[5])}");   // Чемпіон
            }

            Console.WriteLine("===============================================================================================================\n");
        }

        public int GetSimulationMode()
        {
            while (true)
            {
                Console.WriteLine("\n=======================================================");
                Console.WriteLine("             ВИБІР РЕЖИМУ СИМУЛЯЦІЇ");
                Console.WriteLine("=======================================================");
                Console.WriteLine("1 - Звичайна одиночна симуляція з виводом усіх стадій");
                Console.WriteLine("2 - Багаторазова симуляція");
                Console.WriteLine("-------------------------------------------------------");
                Console.Write("Введіть ваш вибір (1 або 2): ");

                string input = Console.ReadLine();

                if (int.TryParse(input, out int mode))
                {
                    if (mode == 1 || mode == 2)
                    {
                        Console.WriteLine("=======================================================\n");
                        return mode;
                    }
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[ПОМИЛКА] Некоректне введення! Будь ласка, введіть число 1 або 2.");
                Console.ResetColor(); 
            }
        }

        public int GetIterationsNumber()
        {
            const int minLimit = 1;
            const int maxLimit = 100000;

            while (true)
            {
                Console.WriteLine("\n=======================================================");
                Console.WriteLine("         ВВЕДЕННЯ КІЛЬКОСТІ ІТЕРАЦІЙ");
                Console.WriteLine("=======================================================");
                Console.WriteLine("Будь ласка, введіть, скільки разів ви хочете просимулювати");
                Console.WriteLine("весь турнір для отримання аналітики.");
                Console.WriteLine($"Ліміт: {minLimit:N0}-{maxLimit:N0}");
                Console.WriteLine("-------------------------------------------------------");
                Console.Write("Введіть кількість: ");

                string input = Console.ReadLine();

                if (int.TryParse(input, out int iterations))
                {
                    if (iterations >= minLimit && iterations <= maxLimit)
                    {
                        Console.WriteLine($"[OK] Обрано {iterations:N0} симуляцій.");
                        Console.WriteLine("=======================================================\n");
                        return iterations;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        if (iterations < minLimit)
                            Console.WriteLine($"\n[ПОМИЛКА] Число занадто мале. Мінімально: {minLimit}.");
                        else
                            Console.WriteLine($"\n[ПОМИЛКА] Число занадто велике. Максимально: {maxLimit:N0}.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ПОМИЛКА] Некоректне введення! Будь ласка, введіть ціле число.");
                    Console.ResetColor();
                }
            }
        }
    }
}
