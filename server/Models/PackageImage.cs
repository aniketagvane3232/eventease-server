namespace server.Models
{
    public class PackageImage
    {
        public int Id { get; set; }

        public int PackageId { get; set; }
        public Package? Package { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
    }
}