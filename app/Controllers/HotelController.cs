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
}
