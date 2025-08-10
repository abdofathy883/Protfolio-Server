using Microsoft.AspNetCore.Http;

namespace Core.DTOs
{
    public class UpdateProjectImageDTO
    {
        public int Id { get; set; } // 0 for new images
        public IFormFile? ImageFile { get; set; } // null for existing images
        public string? Url { get; set; } // Keep existing URL
        public string? AltText { get; set; }
        public bool IsFeatured { get; set; }
        public bool ToDelete { get; set; }
    }
}
