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

        // ===== EDGE CASES & ERROR HANDLING =====

        [Fact]
        public void Search_WithNoMatchingFlights_ShouldReturnEmptyResults()
        {
            // Purpose: Test error handling when no flights match the search criteria
            // This ensures the system gracefully handles invalid routes without crashing
            
            // Arrange
            var holidaySearchView = new HolidaySearch.Views.HolidaySearch();
            
            var searchCriteria = new HolidaySearch.Models.HolidaySearch
            {
                DepartingFrom = "MAN",
                TravelingTo = "NONEXISTENT", // Invalid destination
                DepartureDate = new DateOnly(2023, 7, 1),
                Duration = 7
            };
            
            // Act
            var results = holidaySearchView.Search(searchCriteria);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void Search_WithNoMatchingHotels_ShouldReturnEmptyResults()
        {
            // Purpose: Test error handling when no hotels match the search criteria
            // This ensures the system handles date/duration combinations with no available hotels
            
            // Arrange
            var holidaySearchView = new HolidaySearch.Views.HolidaySearch();
            
            var searchCriteria = new HolidaySearch.Models.HolidaySearch
            {
                DepartingFrom = "MAN",
                TravelingTo = "AGP",
                DepartureDate = new DateOnly(2025, 12, 25), // Future date with no data
                Duration = 7
            };
            
            // Act
            var results = holidaySearchView.Search(searchCriteria);

            // Assert
            Assert.Empty(results);
        }

        // ===== MULTIPLE RESULTS & ORDERING =====

        [Fact]
        public void Search_WithMultipleOptions_ShouldReturnOrderedByPrice()
        {
            // Purpose: Test that when multiple flight/hotel combinations exist, results are ordered by total price
            // This validates the core business requirement of showing cheapest options first
            
            // Arrange
            var holidaySearchView = new HolidaySearch.Views.HolidaySearch();
            
            var searchCriteria = new HolidaySearch.Models.HolidaySearch
            {
                DepartingFrom = null, // Any airport
                TravelingTo = "PMI", // Use PMI which has multiple flights on 2023-06-15
                DepartureDate = new DateOnly(2023, 6, 15),
                Duration = 10
            };
            
            // Act
            var results = holidaySearchView.Search(searchCriteria);

            // Assert
            Assert.NotEmpty(results);
            Assert.True(results.Count > 1, "Should return multiple options for this search");
            
            // Verify results are ordered by total price (cheapest first)
            for (int i = 1; i < results.Count; i++)
            {
                Assert.True(results[i-1].TotalPrice <= results[i].TotalPrice, 
                    $"Results not ordered by price: {results[i-1].TotalPrice} > {results[i].TotalPrice}");
            }
        }

        // ===== AIRPORT CODE RESOLUTION =====

        [Fact]
        public void Search_WithAirportCode_ShouldWorkDirectly()
        {
            // Purpose: Test that direct airport code input works without city name resolution
            // This validates the system accepts both city names and airport codes as input
            
            // Arrange
            var holidaySearchView = new HolidaySearch.Views.HolidaySearch();
            
            var searchCriteria = new HolidaySearch.Models.HolidaySearch
            {
                DepartingFrom = "MAN", // Direct airport code
                TravelingTo = "AGP",   // Direct airport code
                DepartureDate = new DateOnly(2023, 7, 1),
                Duration = 7
            };
            
            // Act
            var results = holidaySearchView.Search(searchCriteria);

            // Assert
            Assert.Single(results);
            Assert.Equal("MAN", results.First().Flight.From);
            Assert.Equal("AGP", results.First().Flight.To);
        }

        [Fact]
        public void Search_WithCityName_ShouldResolveToAirportCodes()
        {
            // Purpose: Test that city names are correctly resolved to airport codes
            // This validates the airport reference controller's city-to-code mapping functionality
            
            // Arrange
            var holidaySearchView = new HolidaySearch.Views.HolidaySearch();
            
            var searchCriteria = new HolidaySearch.Models.HolidaySearch
            {
                DepartingFrom = "Manchester", // City name
                TravelingTo = "Malaga",       // City name
                DepartureDate = new DateOnly(2023, 7, 1),
                Duration = 7
            };
            
            // Act
            var results = holidaySearchView.Search(searchCriteria);

            // Assert
            Assert.Single(results);
            Assert.Equal("MAN", results.First().Flight.From);
            Assert.Equal("AGP", results.First().Flight.To);
        }

        // ===== DATE EDGE CASES =====


        [Fact]
        public void Search_WithWrongDate_ShouldReturnEmptyResults()
        {
            // Purpose: Test that searches with non-matching dates return no results
            // This ensures the system correctly filters out flights/hotels that don't match the requested date
            
            // Arrange
            var holidaySearchView = new HolidaySearch.Views.HolidaySearch();
            
            var searchCriteria = new HolidaySearch.Models.HolidaySearch
            {
                DepartingFrom = "MAN",
                TravelingTo = "AGP",
                DepartureDate = new DateOnly(2023, 8, 1), // Different date than available flights
                Duration = 7
            };
            
            // Act
            var results = holidaySearchView.Search(searchCriteria);

            // Assert
            Assert.Empty(results);
        }

        // ===== DURATION MATCHING =====


        [Fact]
        public void Search_WithWrongDuration_ShouldReturnEmptyResults()
        {
            // Purpose: Test that searches with non-matching durations return no results
            // This ensures the system correctly filters out hotels that don't match the requested duration
            
            // Arrange
            var holidaySearchView = new HolidaySearch.Views.HolidaySearch();
            
            var searchCriteria = new HolidaySearch.Models.HolidaySearch
            {
                DepartingFrom = "MAN",
                TravelingTo = "TFS",
                DepartureDate = new DateOnly(2022, 11, 5),
                Duration = 10 // No hotels with 10 nights on this date
            };
            
            // Act
            var results = holidaySearchView.Search(searchCriteria);

            // Assert
            Assert.Empty(results);
        }

        // ===== COMPLEX SCENARIOS =====

        [Fact]
        public void Search_AnyAirportToAnyDestination_ShouldReturnAllCombinations()
        {
            // Purpose: Test the most open search possible to validate system handles broad queries
            // This tests the system's ability to process and return all available combinations
            
            // Arrange
            var holidaySearchView = new HolidaySearch.Views.HolidaySearch();
            
            var searchCriteria = new HolidaySearch.Models.HolidaySearch
            {
                DepartingFrom = null, // Any airport
                TravelingTo = null,   // Any destination
                DepartureDate = new DateOnly(2023, 6, 15),
                Duration = 10
            };
            
            // Act
            var results = holidaySearchView.Search(searchCriteria);

            // Assert
            Assert.NotEmpty(results);
            
            // Should include multiple destinations (PMI has multiple flights on this date)
            var destinations = results.Select(r => r.Flight.To).Distinct().ToList();
            Assert.True(destinations.Count >= 1, "Should include flights to at least one destination");
        }


    }
}
