using System;
using System.IO;
using System.Linq;
using Xunit;
using HolidaySearch.Controllers;

namespace HolidaySearch.Tests.E2E
{
    public class HolidaySearchE2ETests
    {
        [Fact]
        public void Customer1_ManchesterToMalaga_ShouldReturnFlight2AndHotel9()
        {
            // Arrange - Change working directory to project root so default paths work
            var originalDirectory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory("/home/tom/Dev/holiday-search");

            try
            {
                // Act
                var flightDataController = new HolidaySearch.Controllers.FlightDataController();
                var hotelDataController = new HolidaySearch.Controllers.HotelDataController();
                var flightMatchingController = new HolidaySearch.Controllers.FlightMatchingController();
                var hotelMatchingController = new HolidaySearch.Controllers.HotelMatchingController();
                var holidaySearchController = new HolidaySearch.Controllers.HolidaySearchController();
                
                var flights = flightDataController.GetAllFlights();
                var hotels = hotelDataController.GetAllHotels();
                
                var matchingFlights = flightMatchingController.GetMatchingFlights(
                    flights,
                    new List<string> { "MAN" },
                    new List<string> { "AGP" },
                    new DateOnly(2023, 7, 1)
                );
                
                var matchingHotels = hotelMatchingController.GetMatchingHotels(
                    hotels,
                    new List<string> { "AGP" },
                    new DateOnly(2023, 7, 1),
                    7
                );
                
                var results = holidaySearchController.GetHolidayResults(matchingFlights, matchingHotels);

                // Assert
                Assert.Single(results);

                var result = results.First();

                // Verify Flight 2
                Assert.Equal(2, result.Flight.Id);
                Assert.Equal("Oceanic Airlines", result.Flight.Airline);
                Assert.Equal("MAN", result.Flight.From);
                Assert.Equal("AGP", result.Flight.To);
                Assert.Equal(245, result.Flight.Price);
                Assert.Equal(new DateOnly(2023, 7, 1), result.Flight.DepartureDate);

                // Verify Hotel 9
                Assert.Equal(9, result.Hotel.Id);
                Assert.Equal("Nh Malaga", result.Hotel.Name);
                Assert.Equal(new DateOnly(2023, 7, 1), result.Hotel.ArrivalDate);
                Assert.Equal(83, result.Hotel.PricePerNight);
                Assert.Equal(7, result.Hotel.Nights);
                Assert.Contains("AGP", result.Hotel.LocalAirports);

                // Verify total price calculation
                var expectedTotalPrice = 245 + (83 * 7); // Flight price + (hotel price per night * nights)
                Assert.Equal(expectedTotalPrice, result.TotalPrice);
            }
            finally
            {
                // Restore original working directory
                Directory.SetCurrentDirectory(originalDirectory);
            }
        }
    }
}
