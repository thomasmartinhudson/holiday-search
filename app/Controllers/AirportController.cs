using HolidaySearch.Models;

namespace HolidaySearch.Controllers
{
    public class AirportController
    {
        private readonly JSONController _jsonController;

        public AirportController(string dataPath = "data/airports.json")
        {
            _jsonController = new JSONController(dataPath);
        }

        public List<Airport> GetAllAirports()
        {
            return _jsonController.ReadAll<Airport>();
        }
    }
}
