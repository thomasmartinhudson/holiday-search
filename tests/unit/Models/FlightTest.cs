using System;
using Xunit;
using HolidaySearch.Models;

namespace HolidaySearch.Tests.Unit.Models
{
    public class FlightTest
    {
        [Fact]
        public void Flight_ShouldPopulateCorrectly_WithValidData()
        {
            // Arrange
            var flight = new Flight
            {
                Airline = "Test Airline",
                From = "MAN",
                To = "AGP",
                Price = 125.50m,
                DepartureDate = new DateOnly(2023, 7, 1)
            };

            // Act & Assert
            Assert.Equal("Test Airline", flight.Airline);
            Assert.Equal("MAN", flight.From);
            Assert.Equal("AGP", flight.To);
            Assert.Equal(125.50m, flight.Price);
            Assert.Equal(new DateOnly(2023, 7, 1), flight.DepartureDate);
        }

        [Fact]
        public void Flight_ShouldNotAllowNegativePrice()
        {
            // Arrange
            var flight = new Flight
            {
                Airline = "Test Airline",
                From = "MAN",
                To = "AGP",
                DepartureDate = new DateOnly(2023, 7, 1)
            };

            // Act & Assert - Setting negative price should throw exception
            var exception = Assert.Throws<ArgumentException>(() => flight.Price = -50.00m);
            Assert.Equal("Price cannot be negative (Parameter 'value')", exception.Message);
        }

        [Fact]
        public void Flight_ShouldRoundPriceToTwoDecimalPlaces()
        {
            // Arrange & Act
            var flight = new Flight
            {
                Airline = "Test Airline",
                From = "MAN",
                To = "AGP",
                Price = 123.456789m,
                DepartureDate = new DateOnly(2023, 7, 1)
            };

            // Assert
            Assert.Equal(123.46m, flight.Price);
        }

        [Fact]
        public void Flight_ShouldNotAllowNullAirline()
        {
            // Arrange
            var flight = new Flight();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => flight.Airline = null);
        }

        [Fact]
        public void Flight_ShouldNotAllowNullFrom()
        {
            // Arrange
            var flight = new Flight();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => flight.From = null);
        }

        [Fact]
        public void Flight_ShouldNotAllowNullTo()
        {
            // Arrange
            var flight = new Flight();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => flight.To = null);
        }


        [Fact]
        public void Flight_ShouldNotAllowWrongTypeForAirline()
        {
            // Arrange
            var flight = new Flight();

            // Act & Assert - Cannot assign int to string property
            Assert.Throws<InvalidCastException>(() => flight.Airline = (string)(object)123);
        }

        [Fact]
        public void Flight_ShouldNotAllowWrongTypeForFrom()
        {
            // Arrange
            var flight = new Flight();

            // Act & Assert - Cannot assign int to string property
            Assert.Throws<InvalidCastException>(() => flight.From = (string)(object)123);
        }

        [Fact]
        public void Flight_ShouldNotAllowWrongTypeForTo()
        {
            // Arrange
            var flight = new Flight();

            // Act & Assert - Cannot assign int to string property
            Assert.Throws<InvalidCastException>(() => flight.To = (string)(object)123);
        }

        [Fact]
        public void Flight_ShouldNotAllowWrongTypeForPrice()
        {
            // Arrange
            var flight = new Flight();

            // Act & Assert - Cannot assign string to decimal property
            Assert.Throws<InvalidCastException>(() => flight.Price = (decimal)(object)"invalid");
        }

    }
}
