using System.Text.Json.Serialization;

namespace HolidaySearch.Models
{
    public class Hotel : BaseModel
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("arrival_date")]
        public required DateOnly ArrivalDate { get; set; }

        private decimal _pricePerNight;

        [JsonPropertyName("price_per_night")]
        public decimal PricePerNight
        {
            get => _pricePerNight;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Price per night cannot be negative", nameof(value));
                _pricePerNight = Math.Round(value, 2);
            }
        }

        [JsonPropertyName("local_airports")]
        public required string[] LocalAirports { get; set; } = [];

        private int _nights;

        [JsonPropertyName("nights")]
        public int Nights
        {
            get => _nights;
            set
            {
                if (value < 1)
                    throw new ArgumentException("Nights must be at least 1", nameof(value));
                _nights = value;
            }
        }

        public decimal Price => PricePerNight * Nights;
    }
}
