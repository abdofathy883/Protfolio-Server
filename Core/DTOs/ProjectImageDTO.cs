namespace Core.DTOs
{
    public class ProjectImageDTO
    {
        public int Id { get; set; }
        public required string Url { get; set; }
        public string? AltText { get; set; }
        public bool IsFeatured { get; set; }
        public int ProjectId { get; set; }
    }
}
