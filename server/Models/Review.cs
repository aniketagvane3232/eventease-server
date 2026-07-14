namespace server.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int Rating { get; set; } // 1-5
        public string? Comment { get; set; }
    }
}