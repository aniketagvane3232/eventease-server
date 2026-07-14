namespace server.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Catering, Decoration, DJ, etc.
        public decimal Price { get; set; }

        public ICollection<BookingService>? BookingServices { get; set; }
    }
}