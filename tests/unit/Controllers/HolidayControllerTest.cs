using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using HolidaySearch.Controllers;
using HolidaySearch.Models;

namespace HolidaySearch.Tests.Unit.Controllers
{
    public class HolidaySearchControllerTest
    {
        private readonly Flight[] _testFlights;
        private readonly Hotel[] _testHotels;

        public HolidaySearchControllerTest()
        {
            _testFlights = new Flight[]
            {
                new Flight { Id = 1, Airline = "Test Airline", From = "MAN", To = "AGP", Price = 100.00m, DepartureDate = new DateOnly(2023, 7, 1) },
                new Flight { Id = 2, Airline = "Another Airline", From = "LGW", To = "PMI", Price = 200.00m, DepartureDate = new DateOnly(2023, 8, 15) },
                new Flight { Id = 3, Airline = "Third Airline", From = "MAN", To = "AGP", Price = 150.00m, DepartureDate = new DateOnly(2023, 7, 1) }
            };

            _testHotels = new Hotel[]
            {
                new Hotel { Id = 1, Name = "Test Hotel", ArrivalDate = new DateOnly(2023, 7, 1), PricePerNight = 50.00m, LocalAirports = new string[] { "AGP" }, Nights = 7 },
                new Hotel { Id = 2, Name = "Another Hotel", ArrivalDate = new DateOnly(2023, 8, 15), PricePerNight = 75.00m, LocalAirports = new string[] { "PMI" }, Nights = 14 },
                new Hotel { Id = 3, Name = "Third Hotel", ArrivalDate = new DateOnly(2023, 7, 1), PricePerNight = 60.00m, LocalAirports = new string[] { "AGP" }, Nights = 7 }
            };
        }

        [Fact]
        public void GetHolidayResults_ShouldReturnEmptyList_WhenFlightsListIsEmpty()
        {
            // Arrange
            var controller = new HolidaySearchController();
            var emptyFlights = new Flight[0];
            var hotels = _testHotels.Take(1).ToArray();

            // Act
            var result = controller.GetHolidayResults(emptyFlights, hotels);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetHolidayResults_ShouldReturnEmptyList_WhenHotelsListIsEmpty()
        {
            // Arrange
            var controller = new HolidaySearchController();
            var flights = _testFlights.Take(1).ToArray();
            var emptyHotels = new Hotel[0];

            // Act
            var result = controller.GetHolidayResults(flights, emptyHotels);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetHolidayResults_ShouldReturnEmptyList_WhenBothListsAreEmpty()
        {
            // Arrange
            var controller = new HolidaySearchController();
            var emptyFlights = new Flight[0];
            var emptyHotels = new Hotel[0];

            // Act
            var result = controller.GetHolidayResults(emptyFlights, emptyHotels);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetHolidayResults_ShouldReturnSingleResult_WhenSingleFlightAndHotel()
        {
            // Arrange
            var controller = new HolidaySearchController();
            var singleFlight = _testFlights.Take(1).ToArray();
            var singleHotel = _testHotels.Take(1).ToArray();

            // Act
            var result = controller.GetHolidayResults(singleFlight, singleHotel);

            // Assert
            Assert.Single(result);
            var holidayResult = result.First();
            Assert.Equal(1, holidayResult.Flight.Id);
            Assert.Equal(1, holidayResult.Hotel.Id);
            Assert.Equal(100.00m + 350.00m, holidayResult.TotalPrice); // Flight £100 + Hotel £50/night × 7 nights = £450
        }

        [Fact]
        public void GetHolidayResults_ShouldReturnMultipleResults_WhenMultipleFlightsAndHotels()
        {
            // Arrange
            var controller = new HolidaySearchController();
            var flights = _testFlights.Take(2).ToArray();
            var hotels = _testHotels.Take(2).ToArray();

            // Act
            var result = controller.GetHolidayResults(flights, hotels);

            // Assert
            Assert.Equal(4, result.Length); // 2 flights × 2 hotels = 4 combinations
        }

        [Fact]
        public void GetHolidayResults_ShouldReturnAllCombinations_WhenMultipleFlightsAndHotels()
        {
            // Arrange
            var controller = new HolidaySearchController();
            var flights = _testFlights.Take(2).ToArray();
            var hotels = _testHotels.Take(2).ToArray();

            // Act
            var result = controller.GetHolidayResults(flights, hotels);

            // Assert
            Assert.Equal(4, result.Length);

            // Verify all combinations exist
            var combinations = result.Select(r => new { FlightId = r.Flight.Id, HotelId = r.Hotel.Id }).ToArray();
            Assert.Contains(combinations, c => c.FlightId == 1 && c.HotelId == 1);
            Assert.Contains(combinations, c => c.FlightId == 1 && c.HotelId == 2);
            Assert.Contains(combinations, c => c.FlightId == 2 && c.HotelId == 1);
            Assert.Contains(combinations, c => c.FlightId == 2 && c.HotelId == 2);
        }

        [Fact]
        public void GetHolidayResults_ShouldOrderResultsByPrice_CheapestFirst()
        {
            // Arrange
            var controller = new HolidaySearchController();
            var flights = _testFlights.Take(2).ToArray(); // Flight 1: £100, Flight 2: £200
            var hotels = _testHotels.Take(2).ToArray(); // Hotel 1: £50/night, Hotel 2: £75/night

            // Act
            var result = controller.GetHolidayResults(flights, hotels);

            // Assert
            Assert.Equal(4, result.Length);

            // Verify ordering (cheapest first)
            // Flight 1 + Hotel 1: £100 + (£50 × 7) = £450
            // Flight 1 + Hotel 2: £100 + (£75 × 14) = £1150
            // Flight 2 + Hotel 1: £200 + (£50 × 7) = £550
            // Flight 2 + Hotel 2: £200 + (£75 × 14) = £1250

            Assert.Equal(450.00m, result[0].TotalPrice);
            Assert.Equal(550.00m, result[1].TotalPrice);
            Assert.Equal(1150.00m, result[2].TotalPrice);
            Assert.Equal(1250.00m, result[3].TotalPrice);
        }

        [Fact]
        public void GetHolidayResults_ShouldCalculateTotalPriceCorrectly()
        {
            // Arrange
            var controller = new HolidaySearchController();
            var singleFlight = _testFlights.Take(1).ToArray(); // £100
            var singleHotel = _testHotels.Take(1).ToArray(); // £50/night for 7 nights = £350

            // Act
            var result = controller.GetHolidayResults(singleFlight, singleHotel);

            // Assert
            Assert.Single(result);
            var holidayResult = result.First();
            Assert.Equal(100.00m + 350.00m, holidayResult.TotalPrice); // £450
        }

        [Fact]
        public void GetHolidayResults_ShouldHandleMultipleFlightsWithSamePrice()
        {
            // Arrange
            var controller = new HolidaySearchController();
            var flights = new Flight[]
            {
                new Flight { Id = 1, Airline = "Airline 1", From = "MAN", To = "AGP", Price = 100.00m, DepartureDate = new DateOnly(2023, 7, 1) },
                new Flight { Id = 2, Airline = "Airline 2", From = "LGW", To = "AGP", Price = 100.00m, DepartureDate = new DateOnly(2023, 7, 1) }
            };
            var singleHotel = _testHotels.Take(1).ToArray();

            // Act
            var result = controller.GetHolidayResults(flights, singleHotel);

            // Assert
            Assert.Equal(2, result.Length);
            Assert.All(result, r => Assert.Equal(100.00m + 350.00m, r.TotalPrice));
        }

        [Fact]
        public void GetHolidayResults_ShouldHandleMultipleHotelsWithSamePrice()
        {
            // Arrange
            var controller = new HolidaySearchController();
            var singleFlight = _testFlights.Take(1).ToArray();
            var hotels = new Hotel[]
            {
                new Hotel { Id = 1, Name = "Hotel 1", ArrivalDate = new DateOnly(2023, 7, 1), PricePerNight = 50.00m, LocalAirports = new string[] { "AGP" }, Nights = 7 },
                new Hotel { Id = 2, Name = "Hotel 2", ArrivalDate = new DateOnly(2023, 7, 1), PricePerNight = 50.00m, LocalAirports = new string[] { "AGP" }, Nights = 7 }
            };

            // Act
            var result = controller.GetHolidayResults(singleFlight, hotels);

            // Assert
            Assert.Equal(2, result.Length);
            Assert.All(result, r => Assert.Equal(100.00m + 350.00m, r.TotalPrice));
        }

        [Fact]
        public void GetHolidayResults_ShouldPreserveFlightAndHotelProperties()
        {
            // Arrange
            var controller = new HolidaySearchController();
            var singleFlight = _testFlights.Take(1).ToArray();
            var singleHotel = _testHotels.Take(1).ToArray();

            // Act
            var result = controller.GetHolidayResults(singleFlight, singleHotel);

            // Assert
            Assert.Single(result);
            var holidayResult = result.First();

            // Verify flight properties
            Assert.Equal(1, holidayResult.Flight.Id);
            Assert.Equal("Test Airline", holidayResult.Flight.Airline);
            Assert.Equal("MAN", holidayResult.Flight.From);
            Assert.Equal("AGP", holidayResult.Flight.To);
            Assert.Equal(100.00m, holidayResult.Flight.Price);
            Assert.Equal(new DateOnly(2023, 7, 1), holidayResult.Flight.DepartureDate);

            // Verify hotel properties
            Assert.Equal(1, holidayResult.Hotel.Id);
            Assert.Equal("Test Hotel", holidayResult.Hotel.Name);
            Assert.Equal(new DateOnly(2023, 7, 1), holidayResult.Hotel.ArrivalDate);
            Assert.Equal(50.00m, holidayResult.Hotel.PricePerNight);
            Assert.Equal(7, holidayResult.Hotel.Nights);
            Assert.Contains("AGP", holidayResult.Hotel.LocalAirports);
        }

        [Fact]
        public void GetHolidayResults_ShouldHandleLargeNumberOfCombinations()
        {
            // Arrange
            var controller = new HolidaySearchController();
            var flights = _testFlights; // 3 flights
            var hotels = _testHotels; // 3 hotels

            // Act
            var result = controller.GetHolidayResults(flights, hotels);

            // Assert
            Assert.Equal(9, result.Length); // 3 flights × 3 hotels = 9 combinations
        }

        [Fact]
        public void GetHolidayResults_ShouldMaintainStableOrdering_WhenPricesAreEqual()
        {
            // Arrange
            var controller = new HolidaySearchController();
            var flights = new Flight[]
            {
                new Flight { Id = 1, Airline = "Airline 1", From = "MAN", To = "AGP", Price = 100.00m, DepartureDate = new DateOnly(2023, 7, 1) },
                new Flight { Id = 2, Airline = "Airline 2", From = "LGW", To = "AGP", Price = 100.00m, DepartureDate = new DateOnly(2023, 7, 1) }
            };
            var hotels = new Hotel[]
            {
                new Hotel { Id = 1, Name = "Hotel 1", ArrivalDate = new DateOnly(2023, 7, 1), PricePerNight = 50.00m, LocalAirports = new string[] { "AGP" }, Nights = 7 },
                new Hotel { Id = 2, Name = "Hotel 2", ArrivalDate = new DateOnly(2023, 7, 1), PricePerNight = 50.00m, LocalAirports = new string[] { "AGP" }, Nights = 7 }
            };

            // Act
            var result = controller.GetHolidayResults(flights, hotels);

            // Assert
            Assert.Equal(4, result.Length);
            Assert.All(result, r => Assert.Equal(100.00m + 350.00m, r.TotalPrice));

            // Verify the order is stable (first flight with first hotel, etc.)
            Assert.Equal(1, result[0].Flight.Id);
            Assert.Equal(1, result[0].Hotel.Id);
            Assert.Equal(1, result[1].Flight.Id);
            Assert.Equal(2, result[1].Hotel.Id);
            Assert.Equal(2, result[2].Flight.Id);
            Assert.Equal(1, result[2].Hotel.Id);
            Assert.Equal(2, result[3].Flight.Id);
            Assert.Equal(2, result[3].Hotel.Id);
        }
    }
}
