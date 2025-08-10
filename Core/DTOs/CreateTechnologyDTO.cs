using Microsoft.AspNetCore.Http;

namespace Core.DTOs
{
    public class CreateTechnologyDTO
    {
        public required string Name { get; set; }
        public IFormFile? IconFile { get; set; }
    }
}
