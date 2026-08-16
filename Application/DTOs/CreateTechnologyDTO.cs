using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    public class CreateTechnologyDTO
    {
        public required string Name { get; set; }
        public IFormFile? IconFile { get; set; }
    }
}
