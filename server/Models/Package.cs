namespace server.Models
{
    public class Package
    {
        public int Id { get; set; }
        public int EventTypeId { get; set; }
        public EventType? EventType { get; set; }

        public string PackageName { get; set; } = string.Empty; // Silver, Gold, Platinum
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public bool IsTrending { get; set; } = false;

        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<PackageImage>? Images { get; set; }
    }
}