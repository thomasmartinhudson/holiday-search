using HolidaySearch.Models;
using HolidaySearch.Controllers;

namespace HolidaySearch.Views
{
    public class HolidaySearch
    {
        private readonly HotelDataController _hotelDataController;
        private readonly AirportDataController _airportDataController;
        private readonly FlightDataController _flightDataController;

        public HolidaySearch()
        {
            _hotelDataController = new HotelDataController();
            _airportDataController = new AirportDataController();
            _flightDataController = new FlightDataController();
        }

        public HolidayResult[] Search(Models.HolidaySearch searchCriteria)
        {
            // Step 1: Read all Airport data from the data directory
            var allAirports = _airportDataController.GetAllAirports();

            // Step 2: Get all Airport codes using AirportReferenceController
            var departingFromCodes = AirportReferenceController.GetAirportCodes(searchCriteria.DepartingFrom, allAirports);
            var travelingToCodes = AirportReferenceController.GetAirportCodes(searchCriteria.TravelingTo, allAirports);

            if (departingFromCodes.Length == 0 || travelingToCodes.Length == 0)
            {
                return [];
            }

            // Step 3: Read all Flight data from the data directory
            var allFlights = _flightDataController.GetAllFlights();

            // Step 4: Get matching flights using FlightMatchingController
            var matchingFlights = FlightMatchingController.GetMatchingFlights(
                allFlights,
                departingFromCodes,
                travelingToCodes,
                searchCriteria.DepartureDate
            );

            if (matchingFlights.Length == 0)
            {
                return [];
            }

            // Step 5: Read all Hotel data from the data directory
            var allHotels = _hotelDataController.GetAllHotels();

            // Step 6: Get matching hotels from HotelMatchingController
            var destinationCodes = AirportReferenceController.GetAirportCodes(searchCriteria.TravelingTo, allAirports);
            var matchingHotels = HotelMatchingController.GetMatchingHotels(
                allHotels,
                destinationCodes,
                searchCriteria.DepartureDate,
                searchCriteria.Duration
            );
            if (matchingHotels.Length == 0)
            {
                return [];
            }

            // Step 7: Get holiday results using HolidaySearchController
            var holidayResults = HolidaySearchController.GetHolidayResults(matchingFlights, matchingHotels);

            return holidayResults;
        }
    }
}
