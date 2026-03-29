namespace Core.DTOs.Projects
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string? Slug { get; set; }
        public DateTime PublishedAt { get; set; }
        public string? ImageLink { get; set; }
        public string? VideoLink { get; set; }
        public List<TechnologyDTO> Technologies { get; set; } = new();
        public List<ProjectTranslationDto> Translations { get; set; } = new();
        public string? LiveUrl { get; set; }
        public string? DemoUrl { get; set; }
    }
}
