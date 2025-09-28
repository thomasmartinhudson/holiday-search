using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using HolidaySearch.Services;
using HolidaySearch.Models;

namespace HolidaySearch.Tests.Unit.Services
{
    public class AirportServiceTest
    {
        [Fact]
        public void GetAllAirports_ShouldReturnAllAirports_WhenValidDataFile()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            var jsonContent = @"[
                {
                    ""id"": 1,
                    ""code"": ""MAN"",
                    ""name"": ""Manchester Airport"",
                    ""city"": ""Manchester""
                },
                {
                    ""id"": 2,
                    ""code"": ""LGW"",
                    ""name"": ""London Gatwick Airport"",
                    ""city"": ""London""
                }
            ]";
            File.WriteAllText(tempFile, jsonContent);

            try
            {
                var service = new AirportService(tempFile);

                // Act
                var airports = service.GetAllAirports();

                // Assert
                Assert.Equal(2, airports.Count);
                
                var firstAirport = airports.First();
                Assert.Equal(1, firstAirport.Id);
                Assert.Equal("MAN", firstAirport.Code);
                Assert.Equal("Manchester Airport", firstAirport.Name);
                Assert.Equal("Manchester", firstAirport.City);

                var secondAirport = airports.Last();
                Assert.Equal(2, secondAirport.Id);
                Assert.Equal("LGW", secondAirport.Code);
                Assert.Equal("London Gatwick Airport", secondAirport.Name);
                Assert.Equal("London", secondAirport.City);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void GetAllAirports_ShouldThrowInvalidOperationException_WhenInvalidAirportCode()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            var jsonContent = @"[
                {
                    ""id"": 1,
                    ""code"": ""INVALID"",
                    ""name"": ""Test Airport"",
                    ""city"": ""Test City""
                }
            ]";
            File.WriteAllText(tempFile, jsonContent);

            try
            {
                var service = new AirportService(tempFile);

                // Act & Assert
                var exception = Assert.Throws<InvalidOperationException>(() => service.GetAllAirports());
                Assert.Contains("Error reading data file", exception.Message);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }
    }
}
