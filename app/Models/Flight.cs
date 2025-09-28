using System;

namespace HolidaySearch.Models
{
    public class Flight : BaseModel
    {
        private string _airline;
        public string Airline 
        { 
            get => _airline;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value), "Airline cannot be null");
                _airline = value;
            }
        }

        private string _from;
        public string From 
        { 
            get => _from;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value), "From cannot be null");
                _from = value;
            }
        }

        private string _to;
        public string To 
        { 
            get => _to;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value), "To cannot be null");
                _to = value;
            }
        }
        
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
        
        public DateOnly DepartureDate { get; set; }
    }
}
