using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using HolidaySearch.Controllers;
using HolidaySearch.Models;

namespace HolidaySearch.Tests.Unit.Controllers
{
    public class AirportDataControllerTest
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
                var controller = new AirportDataController(tempFile);

                // Act
                var airports = controller.GetAllAirports();

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
                var controller = new AirportDataController(tempFile);

                // Act & Assert
                var exception = Assert.Throws<InvalidOperationException>(() => controller.GetAllAirports());
                Assert.Contains("Error reading data file", exception.Message);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }
    }

    public class AirportReferenceControllerTest
    {
        private readonly List<Airport> _testAirports;

        public AirportReferenceControllerTest()
        {
            _testAirports = new List<Airport>
            {
                new Airport { Id = 1, Code = "MAN", Name = "Manchester Airport", City = "Manchester" },
                new Airport { Id = 2, Code = "LGW", Name = "London Gatwick Airport", City = "London" },
                new Airport { Id = 3, Code = "AGP", Name = "Malaga Airport", City = "Malaga" },
                new Airport { Id = 4, Code = "PMI", Name = "Palma Airport", City = "Palma" }
            };
        }

        [Fact]
        public void GetAirportCodes_ShouldReturnAllAirportCodes_WhenLocationIsNull()
        {
            // Act
            var result = AirportReferenceController.GetAirportCodes(null, _testAirports);

            // Assert
            Assert.Equal(4, result.Count);
            Assert.Contains("MAN", result);
            Assert.Contains("LGW", result);
            Assert.Contains("AGP", result);
            Assert.Contains("PMI", result);
        }

        [Fact]
        public void GetAirportCodes_ShouldReturnAllAirportCodes_WhenLocationIsEmpty()
        {
            // Act
            var result = AirportReferenceController.GetAirportCodes("", _testAirports);

            // Assert
            Assert.Equal(4, result.Count);
            Assert.Contains("MAN", result);
            Assert.Contains("LGW", result);
            Assert.Contains("AGP", result);
            Assert.Contains("PMI", result);
        }

        [Fact]
        public void GetAirportCodes_ShouldReturnSingleAirportCode_WhenValidAirportCodeProvided()
        {
            // Act
            var result = AirportReferenceController.GetAirportCodes("MAN", _testAirports);

            // Assert
            Assert.Single(result);
            Assert.Equal("MAN", result.First());
        }

        [Fact]
        public void GetAirportCodes_ShouldReturnSingleAirportCode_WhenValidAirportCodeProvided_Lowercase()
        {
            // Act
            var result = AirportReferenceController.GetAirportCodes("man", _testAirports);

            // Assert
            Assert.Single(result);
            Assert.Equal("MAN", result.First());
        }

        [Fact]
        public void GetAirportCodes_ShouldReturnMatchingAirportCodes_WhenCityNameProvided()
        {
            // Act
            var result = AirportReferenceController.GetAirportCodes("Manchester", _testAirports);

            // Assert
            Assert.Single(result);
            Assert.Equal("MAN", result.First());
        }

        [Fact]
        public void GetAirportCodes_ShouldReturnMatchingAirportCodes_WhenCityNameProvided_CaseInsensitive()
        {
            // Act
            var result = AirportReferenceController.GetAirportCodes("manchester", _testAirports);

            // Assert
            Assert.Single(result);
            Assert.Equal("MAN", result.First());
        }

        [Fact]
        public void GetAirportCodes_ShouldReturnMatchingAirportCodes_WhenAirportNameProvided()
        {
            // Act
            var result = AirportReferenceController.GetAirportCodes("Gatwick", _testAirports);

            // Assert
            Assert.Single(result);
            Assert.Equal("LGW", result.First());
        }

        [Fact]
        public void GetAirportCodes_ShouldReturnMatchingAirportCodes_WhenAirportNameProvided_CaseInsensitive()
        {
            // Act
            var result = AirportReferenceController.GetAirportCodes("gatwick", _testAirports);

            // Assert
            Assert.Single(result);
            Assert.Equal("LGW", result.First());
        }

        [Fact]
        public void GetAirportCodes_ShouldReturnEmptyList_WhenNoMatchingLocation()
        {
            // Act
            var result = AirportReferenceController.GetAirportCodes("NonExistentCity", _testAirports);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetAirportCodes_ShouldReturnEmptyList_WhenInvalidAirportCodeFormat()
        {
            // Act
            var result = AirportReferenceController.GetAirportCodes("AB", _testAirports);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetAirportCodes_ShouldReturnEmptyList_WhenInvalidAirportCodeFormat_TooLong()
        {
            // Act
            var result = AirportReferenceController.GetAirportCodes("ABCD", _testAirports);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetAirportCodes_ShouldReturnEmptyList_WhenInvalidAirportCodeFormat_ContainsNumbers()
        {
            // Act
            var result = AirportReferenceController.GetAirportCodes("M1N", _testAirports);

            // Assert
            Assert.Empty(result);
        }
    }
}
