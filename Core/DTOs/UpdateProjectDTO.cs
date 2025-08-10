using Microsoft.AspNetCore.Http;

namespace Core.DTOs
{
    public class UpdateProjectDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public List<UpdateProjectImageDTO> Images { get; set; } = new();
        public IFormFile? VideoFile { get; set; }
        public string? Video { get; set; } // Keep existing video URL
        public required List<string> Technologies { get; set; } = new();
        public string? Client { get; set; }
        public required string Problem { get; set; }
        public required string Solution { get; set; }
        public string? LiveUrl { get; set; }
        public string? DemoUrl { get; set; }
        public bool IsFeatured { get; set; }
    }
}
