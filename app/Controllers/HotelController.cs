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

        public List<Hotel> GetAllHotels()
        {
            return _jsonController.ReadAll<Hotel>();
        }
    }

    public class HotelMatchingController
    {
        public List<Hotel> GetMatchingHotels(List<Hotel> hotels, List<string> travelingTo, DateOnly departureDate, int duration)
        {
            return hotels.Where(h =>
                (!travelingTo.Any() || h.LocalAirports.Any(airport => travelingTo.Contains(airport))) &&
                h.ArrivalDate == departureDate &&
                h.Nights == duration).ToList();
        }
    }
}
