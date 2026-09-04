using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Technologies
{
    public class UpdateTechDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public IFormFile? IconFile { get; set; }
        public string? IconUrl { get; set; }
    }
}
