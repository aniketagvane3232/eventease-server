namespace server.DTOs
{
    public class PackageDto
    {
        public int EventTypeId { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public bool IsTrending { get; set; }
        public List<string> ImageUrls { get; set; } = new();
    }
}