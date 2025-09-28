using HolidaySearch.Models;

namespace HolidaySearch.Services
{
    public class AirportService
    {
        private readonly JSONService _jsonService;

        public AirportService(string dataPath = "data/airports.json")
        {
            _jsonService = new JSONService(dataPath);
        }

        public List<Airport> GetAllAirports()
        {
            return _jsonService.ReadAll<Airport>();
        }
    }
}
