namespace Application.Dtos.Seo
{
    public class SeoMetaDto
    {
        public string? MetaTitle { get; init; }
        public string? MetaDescription { get; init; }
        public string? OgTitle { get; set; }
        public string? OgDescription { get; set; }
        public string? OgImageUrl { get; init; }
        public string? Keywords { get; set; }
        public bool NoIndex { get; init; }
        public bool NoFollow { get; init; }
    }
}
