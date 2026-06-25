using System.Text.Json;
using World_Cup_2026_Simulator.BusinessLogic.Interfaces;
using World_Cup_2026_Simulator.BusinessLogic.Models;
using World_Cup_2026_Simulator.DataAccess.DTOs;

namespace World_Cup_2026_Simulator.DataAccess.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        public TournamentRootDto GetTournamentData()
        {
            string path = "D:\\files\\VS repository\\World Cup 2026 Simulator\\World Cup 2026 Simulator\\Data\\wc2026.json";

            return DeserializeFromFile<TournamentRootDto>(path);
        }

        public List<ThirdPlaceMatrix> GetThirdPlaceMatrices()
        {
            string path = "D:\\files\\VS repository\\World Cup 2026 Simulator\\World Cup 2026 Simulator\\Data\\fifa2026_third_place_matrix.json";

            return DeserializeFromFile<List<ThirdPlaceMatrix>>(path);
        }

        private T DeserializeFromFile<T>(string path)
        {
            string fileContent = ReadFileContent(path);

            var data = JsonSerializer.Deserialize<T>(fileContent);
            ArgumentNullException.ThrowIfNull(data);

            return data;
        }

        private string ReadFileContent(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);

            try
            {
                string fileContent = File.ReadAllText(path);

                return fileContent;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }
    }
}
