using HolidaySearch.Models;

namespace HolidaySearch.Models
{
    public class HolidaySearch
    {
        public string? DepartingFrom { get; set; }
        public string? TravelingTo { get; set; }
        public DateOnly DepartureDate { get; set; }

        private int _duration;
        public int Duration
        {
            get => _duration;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Duration must be greater than 0", nameof(value));
                _duration = value;
            }
        }
    }

    public class HolidayResult
    {
        public Flight Flight { get; set; } = null!;
        public Hotel Hotel { get; set; } = null!;
        public decimal TotalPrice { get; set; }

        public string DepartingFrom => Flight.From;
        public string TravelingTo => Flight.To;
    }
}
