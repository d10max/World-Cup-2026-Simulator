using World_Cup_2026_Simulator.BusinessLogic.Models;
using World_Cup_2026_Simulator.Helpers;

namespace World_Cup_2026_Simulator.BusinessLogic.Simulation
{
    public class MatchSimulator
    {
        public void SimulateMatch(Match match)
        {
            match.HomeGoals = CalculateGoals(match.HomeTeam, match.AwayTeam);
            match.AwayGoals = CalculateGoals(match.AwayTeam, match.HomeTeam);

            if (match.HomeGoals == match.AwayGoals && match.IsKnockout)
            {
                const double extraTimeFactor = 0.25;

                match.HomeGoals += CalculateGoals(match.HomeTeam, match.AwayTeam, extraTimeFactor);
                match.AwayGoals += CalculateGoals(match.AwayTeam, match.HomeTeam, extraTimeFactor);

                if (match.HomeGoals == match.AwayGoals)
                {
                    var penaltyResult = SimulatePenalties();
                    match.HomePenalties = penaltyResult.HomePenalties;
                    match.AwayPenalties = penaltyResult.AwayPenalties;
                }
            }

            match.IsPlayed = true;
        }

        public int CalculateGoals(Team team, Team opponent, double timeFactor = 1.0)
        {
            double roll = Random.Shared.NextDouble();

            const double basexG = 1.25;
            double xG = basexG * (team.PowerIndex / opponent.PowerIndex) * timeFactor;

            double cumulativeProbability = 0.0;
            int maxGoals = 8;

            for (int goals = 0; goals <= maxGoals; goals++)
            {
                double poissonProbability = (Math.Pow(xG, goals) * Math.Exp(-xG)) / Helper.Factorial(goals);

                cumulativeProbability += poissonProbability;

                if (roll <= cumulativeProbability)
                {
                    return goals;
                }
            }

            return maxGoals;
        }

        private (int HomePenalties, int AwayPenalties) SimulatePenalties()
        {
            int homeScore = 0;
            int awayScore = 0;

            int homeKicksTaken = 0;
            int awayKicksTaken = 0;

            const double scoringChance = 0.75;

            for (int round = 1; round <= 5; round++)
            {
                if (Random.Shared.NextDouble() <= scoringChance) homeScore++;
                homeKicksTaken++;

                if (IsPenaltiesOver(homeScore, awayScore, 5 - homeKicksTaken, 5 - awayScore)) 
                    break;

                if (Random.Shared.NextDouble() <= scoringChance) awayScore++;
                awayKicksTaken++;

                if (IsPenaltiesOver(homeScore, awayScore, 5 - homeKicksTaken, 5 - awayScore))
                    break;
            }

            while (homeScore == awayScore)
            {
                if (Random.Shared.NextDouble() <= scoringChance) homeScore++;
                if (Random.Shared.NextDouble() <= scoringChance) awayScore++;
            }

            return (homeScore,  awayScore);
        }

        private bool IsPenaltiesOver(int score1, int score2, int kickLeft1, int kicksLeft2)
        {
            if (score1 > score2 + kicksLeft2) return true;

            if (score2 > score1 + kickLeft1) return true;

            return false;
        }
    }
}
