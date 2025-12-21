using Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Core.DTOs
{
    public class CreateProjectDTO
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int ProjectID { get; set; }
        public Language Language { get; set; }
        public required List<CreateProjectImageDTO> Images { get; set; }
        public IFormFile? VideoFile { get; set; }
        public required List<string> Technologies { get; set; }
        public string? Client { get; set; }
        public required string Problem { get; set; }
        public required string Solution { get; set; }
        public string? LiveUrl { get; set; }
        public string? DemoUrl { get; set; }
        public bool IsFeatured { get; set; }
    }
}
