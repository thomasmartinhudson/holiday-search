using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using HolidaySearch.Controllers;
using HolidaySearch.Models;

namespace HolidaySearch.Tests.Unit.Controllers
{
    public class HotelDataControllerTest
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
                var controller = new HotelDataController(tempFile);

                // Act
                var hotels = controller.GetAllHotels();

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
                var controller = new HotelDataController(tempFile);

                // Act & Assert
                var exception = Assert.Throws<InvalidOperationException>(() => controller.GetAllHotels());
                Assert.Contains("Failed to parse JSON data", exception.Message);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }
    }

    public class HotelMatchingControllerTest
    {
        private readonly List<Hotel> _testHotels;

        public HotelMatchingControllerTest()
        {
            _testHotels = new List<Hotel>
            {
                new Hotel { Id = 1, Name = "Test Hotel", ArrivalDate = new DateOnly(2023, 7, 1), PricePerNight = 50.00m, LocalAirports = new List<string> { "MAN", "LGW" }, Nights = 7 },
                new Hotel { Id = 2, Name = "Another Hotel", ArrivalDate = new DateOnly(2023, 8, 15), PricePerNight = 75.50m, LocalAirports = new List<string> { "PMI" }, Nights = 14 },
                new Hotel { Id = 3, Name = "Third Hotel", ArrivalDate = new DateOnly(2023, 7, 1), PricePerNight = 60.00m, LocalAirports = new List<string> { "AGP" }, Nights = 7 },
                new Hotel { Id = 4, Name = "Fourth Hotel", ArrivalDate = new DateOnly(2023, 8, 15), PricePerNight = 80.00m, LocalAirports = new List<string> { "LGW", "MAN" }, Nights = 14 }
            };
        }

        [Fact]
        public void GetMatchingHotels_ShouldReturnAllHotels_WhenEmptyTravelingToList()
        {
            // Arrange
            var controller = new HotelMatchingController();

            // Act
            var result = controller.GetMatchingHotels(_testHotels, new List<string>(), new DateOnly(2023, 7, 1), 7);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, h => h.Id == 1);
            Assert.Contains(result, h => h.Id == 3);
        }

        [Fact]
        public void GetMatchingHotels_ShouldReturnMatchingHotels_WhenSpecificDestinationAirport()
        {
            // Arrange
            var controller = new HotelMatchingController();

            // Act
            var result = controller.GetMatchingHotels(_testHotels, new List<string> { "AGP" }, new DateOnly(2023, 7, 1), 7);

            // Assert
            Assert.Single(result);
            Assert.Equal(3, result.First().Id);
            Assert.Equal("AGP", result.First().LocalAirports.First());
        }

        [Fact]
        public void GetMatchingHotels_ShouldReturnMatchingHotels_WhenSpecificArrivalDate()
        {
            // Arrange
            var controller = new HotelMatchingController();

            // Act
            var result = controller.GetMatchingHotels(_testHotels, new List<string>(), new DateOnly(2023, 7, 1), 7);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, h => Assert.Equal(new DateOnly(2023, 7, 1), h.ArrivalDate));
        }

        [Fact]
        public void GetMatchingHotels_ShouldReturnMatchingHotels_WhenSpecificDuration()
        {
            // Arrange
            var controller = new HotelMatchingController();

            // Act
            var result = controller.GetMatchingHotels(_testHotels, new List<string>(), new DateOnly(2023, 7, 1), 7);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, h => Assert.Equal(7, h.Nights));
        }

        [Fact]
        public void GetMatchingHotels_ShouldReturnEmptyList_WhenNoMatchingCriteria()
        {
            // Arrange
            var controller = new HotelMatchingController();

            // Act
            var result = controller.GetMatchingHotels(_testHotels, new List<string> { "INVALID" }, new DateOnly(2023, 12, 31), 30);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetMatchingHotels_ShouldReturnMatchingHotels_WhenMultipleDestinationAirports()
        {
            // Arrange
            var controller = new HotelMatchingController();

            // Act
            var result = controller.GetMatchingHotels(_testHotels, new List<string> { "MAN", "LGW" }, new DateOnly(2023, 7, 1), 7);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }

        [Fact]
        public void GetMatchingHotels_ShouldReturnMatchingHotels_WhenAllCriteriaMatch()
        {
            // Arrange
            var controller = new HotelMatchingController();

            // Act
            var result = controller.GetMatchingHotels(_testHotels, new List<string> { "AGP" }, new DateOnly(2023, 7, 1), 7);

            // Assert
            Assert.Single(result);
            Assert.Equal(3, result.First().Id);
            Assert.Contains("AGP", result.First().LocalAirports);
            Assert.Equal(new DateOnly(2023, 7, 1), result.First().ArrivalDate);
            Assert.Equal(7, result.First().Nights);
        }

        [Fact]
        public void GetMatchingHotels_ShouldReturnEmptyList_WhenNoHotelsMatchDate()
        {
            // Arrange
            var controller = new HotelMatchingController();

            // Act
            var result = controller.GetMatchingHotels(_testHotels, new List<string> { "AGP" }, new DateOnly(2023, 12, 31), 7);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetMatchingHotels_ShouldReturnEmptyList_WhenNoHotelsMatchDuration()
        {
            // Arrange
            var controller = new HotelMatchingController();

            // Act
            var result = controller.GetMatchingHotels(_testHotels, new List<string> { "AGP" }, new DateOnly(2023, 7, 1), 30);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetMatchingHotels_ShouldReturnEmptyList_WhenNoHotelsMatchDestination()
        {
            // Arrange
            var controller = new HotelMatchingController();

            // Act
            var result = controller.GetMatchingHotels(_testHotels, new List<string> { "INVALID" }, new DateOnly(2023, 7, 1), 7);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetMatchingHotels_ShouldReturnMatchingHotels_WhenHotelHasMultipleLocalAirports()
        {
            // Arrange
            var controller = new HotelMatchingController();

            // Act
            var result = controller.GetMatchingHotels(_testHotels, new List<string> { "LGW" }, new DateOnly(2023, 7, 1), 7);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
            Assert.Contains("LGW", result.First().LocalAirports);
        }

        [Fact]
        public void GetMatchingHotels_ShouldReturnMatchingHotels_WhenMultipleHotelsMatchSameAirport()
        {
            // Arrange
            var controller = new HotelMatchingController();

            // Act
            var result = controller.GetMatchingHotels(_testHotels, new List<string> { "MAN" }, new DateOnly(2023, 8, 15), 14);

            // Assert
            Assert.Single(result);
            Assert.Equal(4, result.First().Id);
            Assert.Contains("MAN", result.First().LocalAirports);
        }
    }
}
