using System;
using System.IO;
using System.Linq;
using Xunit;
using HolidaySearch.Views;
using HolidaySearch.Models;

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
                var holidaySearchView = new HolidaySearch.Views.HolidaySearch();
                
                var searchCriteria = new HolidaySearch.Models.HolidaySearch
                {
                    DepartingFrom = "Manchester",
                    TravelingTo = "Malaga",
                    DepartureDate = new DateOnly(2023, 7, 1),
                    Duration = 7
                };
                
                var results = holidaySearchView.Search(searchCriteria);

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
