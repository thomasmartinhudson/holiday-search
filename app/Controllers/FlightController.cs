using HolidaySearch.Models;

namespace HolidaySearch.Controllers
{
    public class FlightDataController(string dataPath = "data/flights.json")
    {
        private readonly JSONController _jsonController = new(dataPath);

        public Flight[] GetAllFlights()
        {
            return _jsonController.ReadAll<Flight>();
        }
    }

    public class FlightMatchingController
    {
        public static Flight[] GetMatchingFlights(Flight[] flights, string[] departingFrom, string[] travelingTo, DateOnly departureDate)
        {
            return [.. flights.Where(f =>
                (departingFrom.Length == 0 || departingFrom.Contains(f.From)) &&
                (travelingTo.Length == 0 || travelingTo.Contains(f.To)) &&
                f.DepartureDate == departureDate)];
        }
    }
}
