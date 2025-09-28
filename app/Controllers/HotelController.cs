using HolidaySearch.Models;

namespace HolidaySearch.Controllers
{
    public class HotelDataController
    {
        private readonly JSONController _jsonController;

        public HotelDataController(string dataPath = "data/hotels.json")
        {
            _jsonController = new JSONController(dataPath);
        }

        public Hotel[] GetAllHotels()
        {
            return _jsonController.ReadAll<Hotel>();
        }
    }

    public class HotelMatchingController
    {
        public static Hotel[] GetMatchingHotels(Hotel[] hotels, string[] travelingTo, DateOnly departureDate, int duration)
        {
            return [.. hotels.Where(h =>
                (travelingTo.Length == 0 || h.LocalAirports.Any(airport => travelingTo.Contains(airport))) &&
                h.ArrivalDate == departureDate &&
                h.Nights == duration)];
        }
    }
}
