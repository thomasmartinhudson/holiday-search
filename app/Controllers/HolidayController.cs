using HolidaySearch.Models;

namespace HolidaySearch.Controllers
{
    public class HolidaySearchController
    {
        public static HolidayResult[] GetHolidayResults(Flight[] matchingFlights, Hotel[] matchingHotels)
        {
            if (matchingFlights.Length == 0 || matchingHotels.Length == 0)
            {
                return [];
            }

            // Create holiday combinations
            var results = new List<HolidayResult>(matchingFlights.Length * matchingHotels.Length);
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

            return [.. results.OrderBy(r => r.TotalPrice)];
        }

    }

}
