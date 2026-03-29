using Core.Enums;
using Core.Models;
using Microsoft.AspNetCore.Http;

namespace Core.DTOs.Projects
{
    public class CreateProjectDTO
    {
        public string? Slug { get; set; }
        public DateTime PublishedAt { get; set; }
        public IFormFile? ImageFile { get; set; }
        public IFormFile? VideoFile { get; set; }
        public List<string> Technologies { get; set; } = new();
        public List<CreateProjectTranslationDto> Translations { get; set; } = new();
        public string? LiveUrl { get; set; }
        public string? DemoUrl { get; set; }
    }
}
