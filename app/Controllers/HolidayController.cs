using HolidaySearch.Models;

namespace HolidaySearch.Controllers
{
    public class HolidaySearchController
    {
        public List<HolidayResult> GetHolidayResults(List<Flight> matchingFlights, List<Hotel> matchingHotels)
        {
            if (matchingFlights.Count == 0 || matchingHotels.Count == 0)
            {
                return new List<HolidayResult>();
            }

            // Create holiday combinations
            var results = new List<HolidayResult>(matchingFlights.Count * matchingHotels.Count);
            foreach (var flight in matchingFlights)
            {
                foreach (var hotel in matchingHotels)
                {
                    results.Add(new HolidayResult
                    {
                        Flight = flight,
                        Hotel = hotel,
                        TotalPrice = flight.Price + hotel.Price
                    });
                }
            }

            return results.OrderBy(r => r.TotalPrice).ToList();
        }

    }

}
