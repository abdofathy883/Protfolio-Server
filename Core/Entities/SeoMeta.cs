using Microsoft.EntityFrameworkCore;

namespace Core.Entities
{
    [Owned]
    public class SeoMeta
    {
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? OgTitle { get; set; }
        public string? OgDescription { get; set; }
        public string? OgImageUrl { get; set; }
        public string? CanonicalUrl { get; set; }
        public bool NoIndex { get; set; }
        public bool NoFollow { get; set; }
    }
}
