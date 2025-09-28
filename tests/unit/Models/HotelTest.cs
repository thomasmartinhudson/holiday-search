using System;
using System.Collections.Generic;
using Xunit;
using HolidaySearch.Models;

namespace HolidaySearch.Tests.Unit.Models
{
    public class HotelTest
    {
        [Fact]
        public void Hotel_ShouldPopulateCorrectly_WithValidData()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "Test Hotel",
                ArrivalDate = new DateOnly(2023, 7, 1),
                PricePerNight = 99.99m,
                LocalAirports = new string[] { "MAN", "LTN" },
                Nights = 7
            };

            // Act & Assert
            Assert.Equal("Test Hotel", hotel.Name);
            Assert.Equal(new DateOnly(2023, 7, 1), hotel.ArrivalDate);
            Assert.Equal(99.99m, hotel.PricePerNight);
            Assert.Equal(new string[] { "MAN", "LTN" }, hotel.LocalAirports);
            Assert.Equal(7, hotel.Nights);
        }

        [Fact]
        public void Hotel_ShouldNotAllowNegativePricePerNight()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "Test Hotel",
                ArrivalDate = new DateOnly(2023, 7, 1),
                LocalAirports = new string[] { "MAN" },
                Nights = 3
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => hotel.PricePerNight = -10.00m);
            Assert.Equal("Price per night cannot be negative (Parameter 'value')", exception.Message);
        }

        [Fact]
        public void Hotel_ShouldRoundPricePerNightToTwoDecimalPlaces()
        {
            // Arrange & Act
            var hotel = new Hotel
            {
                Name = "Test Hotel",
                ArrivalDate = new DateOnly(2023, 7, 1),
                PricePerNight = 123.456789m,
                LocalAirports = new string[] { "MAN" },
                Nights = 2
            };

            // Assert
            Assert.Equal(123.46m, hotel.PricePerNight);
        }


        [Fact]
        public void Hotel_ShouldNotAllowNightsLessThanOne()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "Test Hotel",
                ArrivalDate = new DateOnly(2023, 7, 1),
                PricePerNight = 50.00m,
                LocalAirports = new string[] { "MAN" }
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => hotel.Nights = 0);
            Assert.Equal("Nights must be at least 1 (Parameter 'value')", exception.Message);
        }

        [Fact]
        public void Hotel_ShouldNotAllowWrongTypeForName()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "Test Hotel",
                ArrivalDate = new DateOnly(2023, 7, 1),
                LocalAirports = new string[] { "MAN" }
            };

            // Act & Assert - Cannot assign int to string property
            Assert.Throws<InvalidCastException>(() => hotel.Name = (string)(object)123);
        }

        [Fact]
        public void Hotel_ShouldNotAllowWrongTypeForLocalAirports()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "Test Hotel",
                ArrivalDate = new DateOnly(2023, 7, 1),
                LocalAirports = new string[] { "MAN" }
            };

            // Act & Assert - Cannot assign int to List<string> property
            Assert.Throws<InvalidCastException>(() => hotel.LocalAirports = (string[])(object)123);
        }

        [Fact]
        public void Hotel_ShouldNotAllowNullPricePerNight()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "Test Hotel",
                ArrivalDate = new DateOnly(2023, 7, 1),
                LocalAirports = new string[] { "MAN" }
            };

            // Act & Assert - Cannot assign null to decimal property (value type)
            Assert.Throws<NullReferenceException>(() => hotel.PricePerNight = (decimal)(object)null!);
        }

        [Fact]
        public void Hotel_ShouldNotAllowNullArrivalDate()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "Test Hotel",
                ArrivalDate = new DateOnly(2023, 7, 1),
                LocalAirports = new string[] { "MAN" }
            };

            // Act & Assert - Cannot assign null to DateOnly property (value type)
            Assert.Throws<NullReferenceException>(() => hotel.ArrivalDate = (DateOnly)(object)null!);
        }

        [Fact]
        public void Hotel_ShouldNotAllowNullNights()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "Test Hotel",
                ArrivalDate = new DateOnly(2023, 7, 1),
                LocalAirports = new string[] { "MAN" }
            };

            // Act & Assert - Cannot assign null to int property (value type)
            Assert.Throws<NullReferenceException>(() => hotel.Nights = (int)(object)null!);
        }

    }
}

