using HolidaySearch.Models;

namespace HolidaySearch.Controllers
{
    public class FlightController
    {
        private readonly JSONController _jsonController;

        public FlightController(string dataPath = "data/flights.json")
        {
            _jsonController = new JSONController(dataPath);
        }

        public List<Flight> GetAllFlights()
        {
            return _jsonController.ReadAll<Flight>();
        }
    }
}
