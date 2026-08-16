using Core.Enums;

namespace Application.DTOs.Projects
{
    public class ProjectPublicDto
    {
        public int Id { get; set; }
        public string? Slug { get; set; }
        public DateTime PublishedAt { get; set; }
        public string? ImageLink { get; set; }
        public string? VideoLink { get; set; }
        public List<TechnologyDTO> Technologies { get; set; } = new();
        public string? LiveUrl { get; set; }
        public string? DemoUrl { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public string? Excerpt { get; set; }
        public string? Client { get; set; }
        public required string Problem { get; set; }
        public required string Solution { get; set; }
        public string? ImageAltText { get; set; }
        public string? VideoAltText { get; set; }
    }
}
