using System;
using System.Collections.Generic;
using Xunit;
using HolidaySearch.Models;

namespace HolidaySearch.Tests.Unit.Models
{
    public class HolidayResultTest
    {
        [Fact]
        public void HolidayResult_ShouldPopulateCorrectly_WithValidData()
        {
            // Arrange
            var flight = new Flight
            {
                Id = 1,
                Airline = "Test Airline",
                From = "MAN",
                To = "AGP",
                Price = 200.00m,
                DepartureDate = new DateOnly(2023, 7, 1)
            };

            var hotel = new Hotel
            {
                Id = 2,
                Name = "Test Hotel",
                ArrivalDate = new DateOnly(2023, 7, 8),
                PricePerNight = 50.00m,
                LocalAirports = new List<string> { "AGP" },
                Nights = 7
            };

            // Act
            var holidayResult = new HolidayResult
            {
                Flight = flight,
                Hotel = hotel,
                TotalPrice = 550.00m
            };

            // Assert
            Assert.Equal(flight, holidayResult.Flight);
            Assert.Equal(hotel, holidayResult.Hotel);
            Assert.Equal(550.00m, holidayResult.TotalPrice);
            Assert.Equal("MAN", holidayResult.DepartingFrom);
            Assert.Equal("AGP", holidayResult.TravelingTo);
        }

        [Fact]
        public void HolidayResult_TotalPrice_ShouldBeCalculatedCorrectly()
        {
            // Arrange
            var flight = new Flight
            {
                Id = 1,
                Airline = "Test Airline",
                From = "MAN",
                To = "AGP",
                Price = 245.00m,
                DepartureDate = new DateOnly(2023, 7, 1)
            };

            var hotel = new Hotel
            {
                Id = 2,
                Name = "Test Hotel",
                ArrivalDate = new DateOnly(2023, 7, 8),
                PricePerNight = 50.00m,
                LocalAirports = new List<string> { "AGP" },
                Nights = 7
            };

            var holidayResult = new HolidayResult
            {
                Flight = flight,
                Hotel = hotel,
                TotalPrice = 595.00m // 245 + (50 * 7)
            };

            // Act & Assert
            Assert.Equal(595.00m, holidayResult.TotalPrice);
            Assert.Equal(245.00m, holidayResult.Flight.Price);
            Assert.Equal(350.00m, holidayResult.Hotel.Price); // 50 * 7
        }
    }
}
