using HolidaySearch.Models;

namespace HolidaySearch.Controllers
{
    public class AirportDataController
    {
        private readonly JSONController _jsonController;

        public AirportDataController(string dataPath = "data/airports.json")
        {
            _jsonController = new JSONController(dataPath);
        }

        public List<Airport> GetAllAirports()
        {
            return _jsonController.ReadAll<Airport>();
        }
    }

    public class AirportReferenceController
    {
        public List<string> GetAirportCodes(string? location, List<Airport> airports)
        {
            // If no location is provided, return all airport codes (null == "Any")
            if (string.IsNullOrEmpty(location))
                return airports.Select(a => a.Code).ToList();

            // Check if it's already an airport code (3 letters)
            if (location.Length == 3 && location.All(char.IsLetter) && location.All(char.IsUpper))
            {
                return new List<string> { location };
            }

            // Search by city name
            var cityMatches = airports.Where(a =>
                a.City.Equals(location, StringComparison.OrdinalIgnoreCase) ||
                a.Name.Contains(location, StringComparison.OrdinalIgnoreCase))
                .Select(a => a.Code)
                .ToList();

            if (cityMatches.Any())
                return cityMatches;

            return new List<string>();
        }
    }
}
