using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World_Cup_2026_Simulator.BusinessLogic.Models
{
    public class Match
    {
        public Match() { }

        public Match(Team homeTeam, Team awayTeam, bool isKnockout)
        {
            ArgumentNullException.ThrowIfNull(homeTeam);
            ArgumentNullException.ThrowIfNull(awayTeam);

            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            IsKnockout = isKnockout;
        }

        public Team HomeTeam { get; set; }

        public Team AwayTeam { get; set; }

        public bool IsKnockout { get; set; }

        public int HomeGoals { get; set; }

        public int AwayGoals { get; set; }

        public int? HomePenalties { get; set; }

        public int? AwayPenalties { get; set; }

        public bool IsPlayed { get; set; }

        public Team GetKnockoutMatchWinner()
        {
            if (!IsPlayed || !IsKnockout)
            {
                throw new InvalidOperationException();
            }

            if (HomeGoals > AwayGoals) return HomeTeam;
            if (AwayGoals > HomeGoals) return AwayTeam;

            return HomePenalties > AwayPenalties ? HomeTeam : AwayTeam;
            
        }

        public Team GetKnockoutMatchLoser()
        {
            return GetKnockoutMatchWinner() == HomeTeam ? AwayTeam : HomeTeam;
        }
    }
}
