using HolidaySearch.Models;

namespace HolidaySearch.Controllers
{
    public class FlightDataController
    {
        private readonly JSONController _jsonController;

        public FlightDataController(string dataPath = "data/flights.json")
        {
            _jsonController = new JSONController(dataPath);
        }

        public List<Flight> GetAllFlights()
        {
            return _jsonController.ReadAll<Flight>();
        }
    }
}
