using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using HolidaySearch.Services;
using HolidaySearch.Models;

namespace HolidaySearch.Tests.Unit.Services
{
    public class HotelServiceTest
    {
        [Fact]
        public void GetAllHotels_ShouldReturnAllHotels_WhenValidDataFile()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            var jsonContent = @"[
                {
                    ""id"": 1,
                    ""name"": ""Test Hotel"",
                    ""arrival_date"": ""2023-07-01"",
                    ""price_per_night"": 50.00,
                    ""local_airports"": [""MAN"", ""LGW""],
                    ""nights"": 7
                },
                {
                    ""id"": 2,
                    ""name"": ""Another Hotel"",
                    ""arrival_date"": ""2023-08-15"",
                    ""price_per_night"": 75.50,
                    ""local_airports"": [""PMI""],
                    ""nights"": 14
                }
            ]";
            File.WriteAllText(tempFile, jsonContent);

            try
            {
                var service = new HotelService(tempFile);

                // Act
                var hotels = service.GetAllHotels();

                // Assert
                Assert.Equal(2, hotels.Count);
                
                var firstHotel = hotels.First();
                Assert.Equal(1, firstHotel.Id);
                Assert.Equal("Test Hotel", firstHotel.Name);
                Assert.Equal(new DateOnly(2023, 7, 1), firstHotel.ArrivalDate);
                Assert.Equal(50.00m, firstHotel.PricePerNight);
                Assert.Equal(new List<string> { "MAN", "LGW" }, firstHotel.LocalAirports);
                Assert.Equal(7, firstHotel.Nights);

                var secondHotel = hotels.Last();
                Assert.Equal(2, secondHotel.Id);
                Assert.Equal("Another Hotel", secondHotel.Name);
                Assert.Equal(new DateOnly(2023, 8, 15), secondHotel.ArrivalDate);
                Assert.Equal(75.50m, secondHotel.PricePerNight);
                Assert.Equal(new List<string> { "PMI" }, secondHotel.LocalAirports);
                Assert.Equal(14, secondHotel.Nights);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void GetAllHotels_ShouldThrowInvalidOperationException_WhenInvalidDateFormat()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            var jsonContent = @"[
                {
                    ""id"": 1,
                    ""name"": ""Test Hotel"",
                    ""arrival_date"": ""invalid-date"",
                    ""price_per_night"": 50.00,
                    ""local_airports"": [""MAN""],
                    ""nights"": 7
                }
            ]";
            File.WriteAllText(tempFile, jsonContent);

            try
            {
                var service = new HotelService(tempFile);

                // Act & Assert
                var exception = Assert.Throws<InvalidOperationException>(() => service.GetAllHotels());
                Assert.Contains("Failed to parse JSON data", exception.Message);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }
    }
}
