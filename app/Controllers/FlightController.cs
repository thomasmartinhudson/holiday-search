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

    public class FlightMatchingController
    {
        public List<Flight> GetMatchingFlights(List<Flight> flights, List<string> departingFrom, List<string> travelingTo, DateOnly departureDate)
        {
            return flights.Where(f =>
                (!departingFrom.Any() || departingFrom.Contains(f.From)) &&
                (!travelingTo.Any() || travelingTo.Contains(f.To)) &&
                f.DepartureDate == departureDate).ToList();
        }
    }
}
