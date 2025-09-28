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
                LocalAirports = new string[] { "AGP" },
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
                LocalAirports = new string[] { "AGP" },
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

    public class HolidaySearchTest
    {
        [Fact]
        public void HolidaySearch_ShouldPopulateCorrectly_WithValidData()
        {
            // Arrange
            var departingFrom = "MAN";
            var travelingTo = "AGP";
            var departureDate = new DateOnly(2023, 7, 1);
            var duration = 7;

            // Act
            var holidaySearch = new HolidaySearch.Models.HolidaySearch
            {
                DepartingFrom = departingFrom,
                TravelingTo = travelingTo,
                DepartureDate = departureDate,
                Duration = duration
            };

            // Assert
            Assert.Equal(departingFrom, holidaySearch.DepartingFrom);
            Assert.Equal(travelingTo, holidaySearch.TravelingTo);
            Assert.Equal(departureDate, holidaySearch.DepartureDate);
            Assert.Equal(duration, holidaySearch.Duration);
        }

        [Fact]
        public void HolidaySearch_ShouldAllowBothNullDepartingFromAndTravelingTo()
        {
            // Arrange & Act
            var holidaySearch = new HolidaySearch.Models.HolidaySearch
            {
                DepartingFrom = null,
                TravelingTo = null,
                DepartureDate = new DateOnly(2023, 7, 1),
                Duration = 7
            };

            // Assert
            Assert.Null(holidaySearch.DepartingFrom);
            Assert.Null(holidaySearch.TravelingTo);
        }

        [Fact]
        public void HolidaySearch_ShouldThrowArgumentException_WhenDurationIsZero()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var holidaySearch = new HolidaySearch.Models.HolidaySearch
                {
                    DepartingFrom = "MAN",
                    TravelingTo = "AGP",
                    DepartureDate = new DateOnly(2023, 7, 1),
                    Duration = 0
                };
            });

            Assert.Equal("Duration must be greater than 0 (Parameter 'value')", exception.Message);
        }

        [Fact]
        public void HolidaySearch_ShouldThrowArgumentException_WhenDurationIsNegative()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var holidaySearch = new HolidaySearch.Models.HolidaySearch
                {
                    DepartingFrom = "MAN",
                    TravelingTo = "AGP",
                    DepartureDate = new DateOnly(2023, 7, 1),
                    Duration = -5
                };
            });

            Assert.Equal("Duration must be greater than 0 (Parameter 'value')", exception.Message);
        }

    }
}
