using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using HolidaySearch.Controllers;
using HolidaySearch.Models;

namespace HolidaySearch.Tests.Unit.Controllers
{
    public class FlightDataControllerTest
    {
        [Fact]
        public void GetAllFlights_ShouldReturnAllFlights_WhenValidDataFile()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            var jsonContent = @"[
                {
                    ""id"": 1,
                    ""airline"": ""Test Airline"",
                    ""from"": ""MAN"",
                    ""to"": ""AGP"",
                    ""price"": 125.50,
                    ""departure_date"": ""2023-07-01""
                },
                {
                    ""id"": 2,
                    ""airline"": ""Another Airline"",
                    ""from"": ""LGW"",
                    ""to"": ""PMI"",
                    ""price"": 200.00,
                    ""departure_date"": ""2023-08-15""
                }
            ]";
            File.WriteAllText(tempFile, jsonContent);

            try
            {
                var controller = new FlightDataController(tempFile);

                // Act
                var flights = controller.GetAllFlights();

                // Assert
                Assert.Equal(2, flights.Count);

                var firstFlight = flights.First();
                Assert.Equal(1, firstFlight.Id);
                Assert.Equal("Test Airline", firstFlight.Airline);
                Assert.Equal("MAN", firstFlight.From);
                Assert.Equal("AGP", firstFlight.To);
                Assert.Equal(125.50m, firstFlight.Price);
                Assert.Equal(new DateOnly(2023, 7, 1), firstFlight.DepartureDate);

                var secondFlight = flights.Last();
                Assert.Equal(2, secondFlight.Id);
                Assert.Equal("Another Airline", secondFlight.Airline);
                Assert.Equal("LGW", secondFlight.From);
                Assert.Equal("PMI", secondFlight.To);
                Assert.Equal(200.00m, secondFlight.Price);
                Assert.Equal(new DateOnly(2023, 8, 15), secondFlight.DepartureDate);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }



        [Fact]
        public void GetAllFlights_ShouldThrowInvalidOperationException_WhenInvalidDateFormat()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            var jsonContent = @"[
                {
                    ""id"": 1,
                    ""airline"": ""Test Airline"",
                    ""from"": ""MAN"",
                    ""to"": ""AGP"",
                    ""price"": 125.50,
                    ""departure_date"": ""invalid-date""
                }
            ]";
            File.WriteAllText(tempFile, jsonContent);

            try
            {
                var controller = new FlightDataController(tempFile);

                // Act & Assert
                var exception = Assert.Throws<InvalidOperationException>(() => controller.GetAllFlights());
                Assert.Contains("Failed to parse JSON data", exception.Message);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }
    }

    public class FlightMatchingControllerTest
    {
        private readonly List<Flight> _testFlights;

        public FlightMatchingControllerTest()
        {
            _testFlights = new List<Flight>
            {
                new Flight { Id = 1, Airline = "Test Airline", From = "MAN", To = "AGP", Price = 125.50m, DepartureDate = new DateOnly(2023, 7, 1) },
                new Flight { Id = 2, Airline = "Another Airline", From = "LGW", To = "PMI", Price = 200.00m, DepartureDate = new DateOnly(2023, 8, 15) },
                new Flight { Id = 3, Airline = "Third Airline", From = "MAN", To = "PMI", Price = 150.00m, DepartureDate = new DateOnly(2023, 7, 1) },
                new Flight { Id = 4, Airline = "Fourth Airline", From = "LGW", To = "AGP", Price = 175.00m, DepartureDate = new DateOnly(2023, 8, 15) }
            };
        }

        [Fact]
        public void GetMatchingFlights_ShouldReturnAllFlights_WhenEmptyDepartureList()
        {
            // Arrange
            var controller = new FlightMatchingController();

            // Act
            var result = controller.GetMatchingFlights(_testFlights, new List<string>(), new List<string> { "AGP" }, new DateOnly(2023, 7, 1));

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }

        [Fact]
        public void GetMatchingFlights_ShouldReturnAllFlights_WhenEmptyDestinationList()
        {
            // Arrange
            var controller = new FlightMatchingController();

            // Act
            var result = controller.GetMatchingFlights(_testFlights, new List<string> { "MAN" }, new List<string>(), new DateOnly(2023, 7, 1));

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, f => f.Id == 1);
            Assert.Contains(result, f => f.Id == 3);
        }

        [Fact]
        public void GetMatchingFlights_ShouldReturnMatchingFlights_WhenSpecificDepartureAirport()
        {
            // Arrange
            var controller = new FlightMatchingController();

            // Act
            var result = controller.GetMatchingFlights(_testFlights, new List<string> { "MAN" }, new List<string>(), new DateOnly(2023, 7, 1));

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, f => Assert.Equal("MAN", f.From));
        }

        [Fact]
        public void GetMatchingFlights_ShouldReturnMatchingFlights_WhenSpecificDestinationAirport()
        {
            // Arrange
            var controller = new FlightMatchingController();

            // Act
            var result = controller.GetMatchingFlights(_testFlights, new List<string>(), new List<string> { "AGP" }, new DateOnly(2023, 7, 1));

            // Assert
            Assert.Single(result);
            Assert.Equal("AGP", result.First().To);
        }

        [Fact]
        public void GetMatchingFlights_ShouldReturnMatchingFlights_WhenSpecificDepartureDate()
        {
            // Arrange
            var controller = new FlightMatchingController();

            // Act
            var result = controller.GetMatchingFlights(_testFlights, new List<string>(), new List<string>(), new DateOnly(2023, 7, 1));

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, f => Assert.Equal(new DateOnly(2023, 7, 1), f.DepartureDate));
        }

        [Fact]
        public void GetMatchingFlights_ShouldReturnEmptyList_WhenNoMatchingCriteria()
        {
            // Arrange
            var controller = new FlightMatchingController();

            // Act
            var result = controller.GetMatchingFlights(_testFlights, new List<string> { "INVALID" }, new List<string> { "INVALID" }, new DateOnly(2023, 12, 31));

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetMatchingFlights_ShouldReturnMatchingFlights_WhenMultipleDepartureAirports()
        {
            // Arrange
            var controller = new FlightMatchingController();

            // Act
            var result = controller.GetMatchingFlights(_testFlights, new List<string> { "MAN", "LGW" }, new List<string>(), new DateOnly(2023, 7, 1));

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, f => f.Id == 1);
            Assert.Contains(result, f => f.Id == 3);
        }

        [Fact]
        public void GetMatchingFlights_ShouldReturnMatchingFlights_WhenMultipleDestinationAirports()
        {
            // Arrange
            var controller = new FlightMatchingController();

            // Act
            var result = controller.GetMatchingFlights(_testFlights, new List<string>(), new List<string> { "AGP", "PMI" }, new DateOnly(2023, 7, 1));

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, f => f.To == "AGP");
            Assert.Contains(result, f => f.To == "PMI");
        }

        [Fact]
        public void GetMatchingFlights_ShouldReturnMatchingFlights_WhenAllCriteriaMatch()
        {
            // Arrange
            var controller = new FlightMatchingController();

            // Act
            var result = controller.GetMatchingFlights(_testFlights, new List<string> { "MAN" }, new List<string> { "AGP" }, new DateOnly(2023, 7, 1));

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
            Assert.Equal("MAN", result.First().From);
            Assert.Equal("AGP", result.First().To);
            Assert.Equal(new DateOnly(2023, 7, 1), result.First().DepartureDate);
        }

        [Fact]
        public void GetMatchingFlights_ShouldReturnEmptyList_WhenNoFlightsMatchDate()
        {
            // Arrange
            var controller = new FlightMatchingController();

            // Act
            var result = controller.GetMatchingFlights(_testFlights, new List<string> { "MAN" }, new List<string> { "AGP" }, new DateOnly(2023, 12, 31));

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetMatchingFlights_ShouldReturnEmptyList_WhenNoFlightsMatchDeparture()
        {
            // Arrange
            var controller = new FlightMatchingController();

            // Act
            var result = controller.GetMatchingFlights(_testFlights, new List<string> { "INVALID" }, new List<string> { "AGP" }, new DateOnly(2023, 7, 1));

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetMatchingFlights_ShouldReturnEmptyList_WhenNoFlightsMatchDestination()
        {
            // Arrange
            var controller = new FlightMatchingController();

            // Act
            var result = controller.GetMatchingFlights(_testFlights, new List<string> { "MAN" }, new List<string> { "INVALID" }, new DateOnly(2023, 7, 1));

            // Assert
            Assert.Empty(result);
        }
    }
}
