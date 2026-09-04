namespace Application.DTOs.Technologies
{
    public class TechnologyDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? IconUrl { get; set; }
    }
}
