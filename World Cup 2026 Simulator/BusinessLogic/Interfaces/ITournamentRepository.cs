using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World_Cup_2026_Simulator.BusinessLogic.Models;
using World_Cup_2026_Simulator.DataAccess.DTOs;

namespace World_Cup_2026_Simulator.BusinessLogic.Interfaces
{
    public interface ITournamentRepository
    {
        TournamentRootDto GetTournamentData();

        List<ThirdPlaceMatrix> GetThirdPlaceMatrices();
    }
}
