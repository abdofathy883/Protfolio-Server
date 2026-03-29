using Core.Enums;
using Core.Models;

namespace Core.DTOs.Projects
{
    public class CreateProjectTranslationDto
    {
        public int ProjectID { get; set; }
        public Language Language { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Client { get; set; }
        public required string Problem { get; set; }
        public required string Solution { get; set; }
        public string? ImageAltText { get; set; }
        public string? VideoAltText { get; set; }
    }
}
