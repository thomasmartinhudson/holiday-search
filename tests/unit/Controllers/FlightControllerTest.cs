using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using HolidaySearch.Controllers;
using HolidaySearch.Models;

namespace HolidaySearch.Tests.Unit.Controllers
{
    public class FlightControllerTest
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
                var controller = new FlightController(tempFile);

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
                var controller = new FlightController(tempFile);

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
}
