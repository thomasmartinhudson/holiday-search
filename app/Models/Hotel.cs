using System;
using System.Collections.Generic;

namespace HolidaySearch.Models
{
    public class Hotel : BaseModel
    {
        public required string Name { get; set; }

        public required DateOnly ArrivalDate { get; set; }

        private decimal _pricePerNight;
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

        public required List<string> LocalAirports { get; set; } = new();

        private int _nights;
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
    }
}
