using Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Slug { get; set; }
        public DateTime PublishedAt { get; set; }
        public string? ImageLink { get; set; }
        public string? VideoLink { get; set; }
        public List<Technology> Technologies { get; set; } = new();
        public List<ProjectTranslation> Translations { get; set; } = new();
        public string? LiveUrl { get; set; }
        public string? DemoUrl { get; set; }
    }
}
