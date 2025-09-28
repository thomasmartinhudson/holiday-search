using HolidaySearch.Models;

namespace HolidaySearch.Controllers
{
    public class AirportDataController(string dataPath = "data/airports.json")
    {
        private readonly JSONController _jsonController = new(dataPath);

        public Airport[] GetAllAirports()
        {
            return _jsonController.ReadAll<Airport>();
        }
    }

    public class AirportReferenceController
    {
        public static string[] GetAirportCodes(string? location, Airport[] airports)
        {
            // If no location is provided, return all airport codes (null == "Any")
            if (string.IsNullOrEmpty(location))
                return [.. airports.Select(a => a.Code)];

            // Check if it's already an airport code (3 letters)
            if (location.Length == 3 && location.All(char.IsLetter) && location.All(char.IsUpper))
            {
                return [location];
            }

            // Search by city name
            var cityMatches = airports.Where(a =>
                a.City.Equals(location, StringComparison.OrdinalIgnoreCase) ||
                a.Name.Contains(location, StringComparison.OrdinalIgnoreCase))
                .Select(a => a.Code)
                .ToArray();

            if (cityMatches.Length > 0)
                return cityMatches;

            return [];
        }
    }
}
