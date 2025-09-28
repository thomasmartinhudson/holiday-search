using System;
using Xunit;
using HolidaySearch.Models;

namespace HolidaySearch.Tests.Unit.Models
{
    public class AirportTest
    {
        [Fact]
        public void Airport_ShouldPopulateCorrectly_WithValidData()
        {
            // Arrange
            var airport = new Airport
            {
                Code = "MAN",
                Name = "Manchester Airport",
                City = "Manchester"
            };

            // Act & Assert
            Assert.Equal("MAN", airport.Code);
            Assert.Equal("Manchester Airport", airport.Name);
            Assert.Equal("Manchester", airport.City);
        }

        [Fact]
        public void Airport_ShouldConvertCodeToUppercase()
        {
            // Arrange
            var airport = new Airport
            {
                Code = "man",
                Name = "Manchester Airport",
                City = "Manchester"
            };

            // Act & Assert
            Assert.Equal("MAN", airport.Code);
        }

        [Fact]
        public void Airport_ShouldThrowArgumentException_WhenCodeIsNull()
        {
            // Arrange
            var airport = new Airport
            {
                Name = "Manchester Airport",
                City = "Manchester"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => airport.Code = null!);
            Assert.Contains("Airport code cannot be null or empty", exception.Message);
        }

        [Fact]
        public void Airport_ShouldThrowArgumentException_WhenCodeIsEmpty()
        {
            // Arrange
            var airport = new Airport
            {
                Name = "Manchester Airport",
                City = "Manchester"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => airport.Code = "");
            Assert.Contains("Airport code cannot be null or empty", exception.Message);
        }

        [Fact]
        public void Airport_ShouldThrowArgumentException_WhenCodeIsTooShort()
        {
            // Arrange
            var airport = new Airport
            {
                Name = "Manchester Airport",
                City = "Manchester"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => airport.Code = "MA");
            Assert.Contains("Airport code must be exactly 3 letters", exception.Message);
        }

        [Fact]
        public void Airport_ShouldThrowArgumentException_WhenCodeIsTooLong()
        {
            // Arrange
            var airport = new Airport
            {
                Name = "Manchester Airport",
                City = "Manchester"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => airport.Code = "MANN");
            Assert.Contains("Airport code must be exactly 3 letters", exception.Message);
        }

        [Fact]
        public void Airport_ShouldThrowArgumentException_WhenCodeContainsNumbers()
        {
            // Arrange
            var airport = new Airport
            {
                Name = "Manchester Airport",
                City = "Manchester"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => airport.Code = "M4N");
            Assert.Contains("Airport code must contain only letters", exception.Message);
        }

        [Fact]
        public void Airport_ShouldThrowArgumentException_WhenCodeContainsSpecialCharacters()
        {
            // Arrange
            var airport = new Airport
            {
                Name = "Manchester Airport",
                City = "Manchester"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => airport.Code = "M-N");
            Assert.Contains("Airport code must contain only letters", exception.Message);
        }

    }
}
