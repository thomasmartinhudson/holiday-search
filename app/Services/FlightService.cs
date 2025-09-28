using HolidaySearch.Models;

namespace HolidaySearch.Services
{
    public class FlightService
    {
        private readonly JSONService _jsonService;

        public FlightService(string dataPath = "data/flights.json")
        {
            _jsonService = new JSONService(dataPath);
        }

        public List<Flight> GetAllFlights()
        {
            return _jsonService.ReadAll<Flight>();
        }
    }
}