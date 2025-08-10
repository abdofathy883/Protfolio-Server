using Microsoft.AspNetCore.Http;

namespace Core.DTOs
{
    public class UpdateTechDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public IFormFile? IconFile { get; set; }
        public string? IconUrl { get; set; }
    }
}
