using System;
using System.Text.Json.Serialization;

namespace HolidaySearch.Models
{
    public class Airport : BaseModel
    {
        private string _code = string.Empty;

        [JsonPropertyName("code")]
        public string Code
        {
            get => _code;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Airport code cannot be null or empty", nameof(value));

                if (value.Length != 3)
                    throw new ArgumentException("Airport code must be exactly 3 letters", nameof(value));

                if (!value.All(char.IsLetter))
                    throw new ArgumentException("Airport code must contain only letters", nameof(value));

                _code = value.ToUpperInvariant();
            }
        }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("city")]
        public required string City { get; set; }
    }
}
