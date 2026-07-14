namespace server.DTOs
{
    public class AdminDashboardDto
    {
        public int TotalUsers { get; set; }

        public int TotalBookings { get; set; }

        public int PendingBookings { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}