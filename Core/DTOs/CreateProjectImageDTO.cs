using Microsoft.AspNetCore.Http;

namespace Core.DTOs
{
    public class CreateProjectImageDTO
    {
        public required IFormFile ImageFile { get; set; }
        public string? AltText { get; set; }
        public bool IsFeatured { get; set; }
    }
}
