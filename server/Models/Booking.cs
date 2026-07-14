namespace server.Models
{
    public class Booking
    {
        public int Id { get; set; }

        // User Information
        public int UserId { get; set; }
        public User? User { get; set; }

        // Event Information
        public int EventTypeId { get; set; }
        public EventType? EventType { get; set; }

        // Selected Package
        public int PackageId { get; set; }
        public Package? Package { get; set; }

        // Booking Details
        public DateTime EventDate { get; set; }
        public int Guests { get; set; }
        public string? Venue { get; set; }
        public string? SpecialRequest { get; set; }

        // Price
        public decimal TotalAmount { get; set; }

        // Status
        public string Status { get; set; } = "Pending";

        // Audit
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Additional Services
        public ICollection<BookingService>? BookingServices { get; set; }
    }
}