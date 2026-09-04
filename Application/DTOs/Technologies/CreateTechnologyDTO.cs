using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Technologies
{
    public class CreateTechnologyDTO
    {
        public required string Name { get; set; }
        public IFormFile? IconFile { get; set; }
    }
}
