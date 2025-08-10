using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required List<ProjectImage> Images { get; set; } = new();
        public string? Video { get; set; }
        public required List<string> Technologies { get; set; } = new();
        public string? Client { get; set; }
        public required string Problem { get; set; }
        public required string Solution { get; set; }
        public string? LiveUrl { get; set; }
        public string? DemoUrl { get; set; }
        public bool IsFeatured { get; set; }
    }
}
