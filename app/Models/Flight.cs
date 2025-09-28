using System;

namespace HolidaySearch.Models
{
    public class Flight : BaseModel
    {
        public required string Airline { get; set; }
        public required string From { get; set; }
        public required string To { get; set; }
        
        private decimal _price;
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
        
        public required DateOnly DepartureDate { get; set; }
    }
}
