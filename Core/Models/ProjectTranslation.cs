using Core.Enums;

namespace Core.Models
{
    public class ProjectTranslation
    {
        public int Id { get; set; }
        public int ProjectID { get; set; }
        public Project Project { get; set; } = default!;
        public Language Language { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public string? ImageAltText { get; set; }
        public string? VideoAltText { get; set; }
        public string? Client { get; set; }
        public required string Problem { get; set; }
        public required string Solution { get; set; }

    }
}
