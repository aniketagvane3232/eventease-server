namespace server.Models
{
    public class AvailableDate
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}