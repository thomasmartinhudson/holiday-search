using HolidaySearch.Models;

namespace HolidaySearch.Services
{
    public class HotelService
    {
        private readonly JSONService _jsonService;

        public HotelService(string dataPath = "data/hotels.json")
        {
            _jsonService = new JSONService(dataPath);
        }

        public List<Hotel> GetAllHotels()
        {
            return _jsonService.ReadAll<Hotel>();
        }
    }
}
