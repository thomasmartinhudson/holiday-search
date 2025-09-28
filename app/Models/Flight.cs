using System;
using System.Text.Json.Serialization;

namespace HolidaySearch.Models
{
    public class Flight : BaseModel
    {
        [JsonPropertyName("airline")]
        public required string Airline { get; set; }
        
        [JsonPropertyName("from")]
        public required string From { get; set; }
        
        [JsonPropertyName("to")]
        public required string To { get; set; }
        
        private decimal _price;
        
        [JsonPropertyName("price")]
        public decimal Price 
        { 
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative", nameof(value));
                _price = Math.Round(value, 2);
            }
        }
        
        [JsonPropertyName("departure_date")]
        public required DateOnly DepartureDate { get; set; }
    }
}
