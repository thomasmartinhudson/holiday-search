using HolidaySearch.Models;

namespace HolidaySearch.Models
{
    public class HolidayResult
    {
        public Flight Flight { get; set; } = null!;
        public Hotel Hotel { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        
        public string DepartingFrom => Flight.From;
        public string TravelingTo => Flight.To;
    }
}
