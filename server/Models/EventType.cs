namespace server.Models
{
    public class EventType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Wedding, Birthday, Corporate, etc.
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public string? Image { get; set; }

        public ICollection<Package>? Packages { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
    }
}