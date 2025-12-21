using Core.Enums;

namespace Core.DTOs
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int ProjectId { get; set; }
        public Language Language { get; set; }
        public required List<ProjectImageDTO> Images { get; set; }
        public string? Video { get; set; }
        public required List<string> Technologies { get; set; }
        public string? Client { get; set; }
        public required string Problem { get; set; }
        public required string Solution { get; set; }
        public string? LiveUrl { get; set; }
        public string? DemoUrl { get; set; }
        public bool IsFeatured { get; set; }
    }
}
