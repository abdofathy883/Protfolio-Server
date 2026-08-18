using Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Seo
{
    public class UpdateSeoDto
    {
        public int Id { get; set; }
        public string Route { get; set; } = string.Empty;
        public Language Language { get; set; } = Language.en;
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Keywords { get; set; }
        public string? OgTitle { get; set; }
        public string? OgDescription { get; set; }
        public IFormFile? OgImage { get; set; }
        public string? CanonicalUrl { get; set; }
        public string? Robots { get; set; }
    }
}
