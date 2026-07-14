using System;

namespace server.Models
{
    public class ChatLog
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}