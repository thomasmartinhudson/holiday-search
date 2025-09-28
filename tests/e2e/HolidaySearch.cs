using System;
using System.IO;
using System.Linq;
using Xunit;
using HolidaySearch.Views;
using HolidaySearch.Models;

namespace HolidaySearch.Tests.E2E
{
    public class HolidaySearchE2ETests : IDisposable
    {
        private readonly string _originalDirectory;

        public HolidaySearchE2ETests()
        {
            // Set working directory to project root for relative paths
            _originalDirectory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory("../../../../");
        }

        public void Dispose()
        {
            // Restore original working directory
            Directory.SetCurrentDirectory(_originalDirectory);
        }
        [Fact]
        public void Customer1_ManchesterToMalaga_ShouldReturnFlight2AndHotel9()
        {
            // Arrange
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

        [Fact]
        public void Customer2_AnyLondonToMallorca_ShouldReturnFlight6AndHotel5()
        {
            // Arrange
            var holidaySearchView = new HolidaySearch.Views.HolidaySearch();
            
            var searchCriteria = new HolidaySearch.Models.HolidaySearch
            {
                DepartingFrom = "London",
                TravelingTo = "Mallorca",
                DepartureDate = new DateOnly(2023, 6, 15),
                Duration = 10
            };
            
            var results = holidaySearchView.Search(searchCriteria);

            // Assert
            Assert.NotEmpty(results);

            // Get the cheapest result (first in the ordered list)
            var result = results.First();

            // Verify Flight 6
            Assert.Equal(6, result.Flight.Id);
            Assert.Equal("Fresh Airways", result.Flight.Airline);
            Assert.Equal("LGW", result.Flight.From);
            Assert.Equal("PMI", result.Flight.To);
            Assert.Equal(75, result.Flight.Price);
            Assert.Equal(new DateOnly(2023, 6, 15), result.Flight.DepartureDate);

            // Verify Hotel 5
            Assert.Equal(5, result.Hotel.Id);
            Assert.Equal("Sol Katmandu Park & Resort", result.Hotel.Name);
            Assert.Equal(new DateOnly(2023, 6, 15), result.Hotel.ArrivalDate);
            Assert.Equal(60, result.Hotel.PricePerNight);
            Assert.Equal(10, result.Hotel.Nights);
            Assert.Contains("PMI", result.Hotel.LocalAirports);

            // Verify total price calculation
            var expectedTotalPrice = 75 + (60 * 10); // Flight price + (hotel price per night * nights)
            Assert.Equal(expectedTotalPrice, result.TotalPrice);
        }

        [Fact]
        public void Customer3_AnyAirportToGranCanaria_ShouldReturnFlight7AndHotel6()
        {
            // Arrange
            var holidaySearchView = new HolidaySearch.Views.HolidaySearch();
            
            var searchCriteria = new HolidaySearch.Models.HolidaySearch
            {
                DepartingFrom = null, // Any Airport
                TravelingTo = "Gran Canaria",
                DepartureDate = new DateOnly(2022, 11, 10),
                Duration = 14
            };
            
            var results = holidaySearchView.Search(searchCriteria);

            // Assert
            Assert.Single(results);

            var result = results.First();

            // Verify Flight 7
            Assert.Equal(7, result.Flight.Id);
            Assert.Equal("Trans American Airlines", result.Flight.Airline);
            Assert.Equal("MAN", result.Flight.From);
            Assert.Equal("LPA", result.Flight.To);
            Assert.Equal(125, result.Flight.Price);
            Assert.Equal(new DateOnly(2022, 11, 10), result.Flight.DepartureDate);

            // Verify Hotel 6
            Assert.Equal(6, result.Hotel.Id);
            Assert.Equal("Club Maspalomas Suites and Spa", result.Hotel.Name);
            Assert.Equal(new DateOnly(2022, 11, 10), result.Hotel.ArrivalDate);
            Assert.Equal(75, result.Hotel.PricePerNight);
            Assert.Equal(14, result.Hotel.Nights);
            Assert.Contains("LPA", result.Hotel.LocalAirports);

            // Verify total price calculation
            var expectedTotalPrice = 125 + (75 * 14); // Flight price + (hotel price per night * nights)
            Assert.Equal(expectedTotalPrice, result.TotalPrice);
        }
    }
}
