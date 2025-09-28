using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using HolidaySearch.Controllers;

namespace HolidaySearch.Tests.Unit.Controllers
{
    public class JSONControllerTest
    {
        [Fact]
        public void ReadAll_ShouldReturnAllItems_WhenValidDataFile()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            var jsonContent = @"[
                {
                    ""id"": 1,
                    ""name"": ""Test Item 1"",
                    ""value"": 100
                },
                {
                    ""id"": 2,
                    ""name"": ""Test Item 2"",
                    ""value"": 200
                }
            ]";
            File.WriteAllText(tempFile, jsonContent);

            try
            {
                var controller = new JSONController(tempFile);

                // Act
                var items = controller.ReadAll<TestItem>();

                // Assert
                Assert.Equal(2, items.Length);

                var firstItem = items.First();
                Assert.Equal(1, firstItem.Id);
                Assert.Equal("Test Item 1", firstItem.Name);
                Assert.Equal(100, firstItem.Value);

                var secondItem = items.Last();
                Assert.Equal(2, secondItem.Id);
                Assert.Equal("Test Item 2", secondItem.Name);
                Assert.Equal(200, secondItem.Value);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void ReadAll_ShouldReturnEmptyList_WhenFileContainsEmptyArray()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "[]");

            try
            {
                var controller = new JSONController(tempFile);

                // Act
                var items = controller.ReadAll<TestItem>();

                // Assert
                Assert.Empty(items);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void ReadAll_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
        {
            // Arrange
            var controller = new JSONController("nonexistent.json");

            // Act & Assert
            var exception = Assert.Throws<FileNotFoundException>(() => controller.ReadAll<TestItem>());
            Assert.Contains("Data file not found", exception.Message);
        }

        [Fact]
        public void ReadAll_ShouldThrowInvalidOperationException_WhenInvalidJson()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "invalid json content");

            try
            {
                var controller = new JSONController(tempFile);

                // Act & Assert
                var exception = Assert.Throws<InvalidOperationException>(() => controller.ReadAll<TestItem>());
                Assert.Contains("Failed to parse JSON data", exception.Message);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void ReadSingle_ShouldReturnSingleItem_WhenValidDataFile()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            var jsonContent = @"{
                ""id"": 1,
                ""name"": ""Test Item"",
                ""value"": 100
            }";
            File.WriteAllText(tempFile, jsonContent);

            try
            {
                var controller = new JSONController(tempFile);

                // Act
                var item = controller.ReadSingle<TestItem>();

                // Assert
                Assert.Equal(1, item.Id);
                Assert.Equal("Test Item", item.Name);
                Assert.Equal(100, item.Value);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void ReadSingle_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
        {
            // Arrange
            var controller = new JSONController("nonexistent.json");

            // Act & Assert
            var exception = Assert.Throws<FileNotFoundException>(() => controller.ReadSingle<TestItem>());
            Assert.Contains("Data file not found", exception.Message);
        }

        [Fact]
        public void ReadSingle_ShouldThrowInvalidOperationException_WhenInvalidJson()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "invalid json content");

            try
            {
                var controller = new JSONController(tempFile);

                // Act & Assert
                var exception = Assert.Throws<InvalidOperationException>(() => controller.ReadSingle<TestItem>());
                Assert.Contains("Failed to parse JSON data", exception.Message);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void ReadSingle_ShouldThrowInvalidOperationException_WhenNullResult()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "null");

            try
            {
                var controller = new JSONController(tempFile);

                // Act & Assert
                var exception = Assert.Throws<InvalidOperationException>(() => controller.ReadSingle<TestItem>());
                Assert.Contains("Failed to deserialize data", exception.Message);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        private class TestItem
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public int Value { get; set; }
        }
    }
}
