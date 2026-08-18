using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Core.Enums;
using Core.Models;
using Application.Interfaces;
using Application.DTOs.Seo;

namespace Infrastructure.Services
{
    public class SeoService : ISeoService
    {
        private readonly PortfolioDbContext context;
        private readonly MediaUploadService mediaUploadService;

        public SeoService(PortfolioDbContext context, MediaUploadService mediaUploadService)
        {
            this.context = context;
            this.mediaUploadService = mediaUploadService;
        }

        public async Task<SeoContentDTO> CreateSeoContent(CreateSeoContentDTO newContent)
        {
            var seoContent = new Core.Models.SeoContent
            {
                Route = newContent.Route,
                Language = newContent.Language,
                Title = newContent.Title,
                Description = newContent.Description,
                Keywords = newContent.Keywords,
                OgTitle = newContent.OgTitle,
                OgDescription = newContent.OgDescription,
                CanonicalUrl = newContent.CanonicalUrl,
                Robots = newContent.Robots,
                CreatedAt = DateTime.UtcNow
            };

            if (newContent.OgImage != null)
            {
                var uploadResult = await mediaUploadService.UploadImageWithPath(newContent.OgImage, "SEO_" + newContent.Route.Replace("/", "_"));
                seoContent.OgImage = uploadResult.Url;
            }

            context.SeoContents.Add(seoContent);
            await context.SaveChangesAsync();

            return MapToDto(seoContent);
        }

        public async Task<SeoContentDTO> GetContentByRoute(string route, Language language)
        {
            var sanitizedRoute = route.StartsWith("/") ? route : "/" + route;
            if (sanitizedRoute == "//") sanitizedRoute = "/";

            var content = await context.SeoContents
                .FirstOrDefaultAsync(x => x.Route == sanitizedRoute && x.Language == language)
                ?? throw new KeyNotFoundException();

            return MapToDto(content);
        }

        public async Task<SeoContentDTO> UpdateSeoContent(UpdateSeoDto content)
        {
            var existingContent = await context.SeoContents
                .FirstOrDefaultAsync(x => x.Route == content.Route && x.Language == content.Language);

            //if (existingContent == null)
            //{
            //    return await CreateSeoContent(content);
            //}

            existingContent.Title = content.Title;
            existingContent.Description = content.Description;
            existingContent.Keywords = content.Keywords;
            existingContent.OgTitle = content.OgTitle;
            existingContent.OgDescription = content.OgDescription;
            existingContent.CanonicalUrl = content.CanonicalUrl;
            existingContent.Robots = content.Robots;
            existingContent.UpdatedAt = DateTime.UtcNow;

            if (content.OgImage != null)
            {
                var uploadResult = await mediaUploadService.UploadImageWithPath(content.OgImage, "SEO_" + content.Route.Replace("/", "_"));
                existingContent.OgImage = uploadResult.Url;
            }

            context.SeoContents.Update(existingContent);
            await context.SaveChangesAsync();
            return MapToDto(existingContent);
        }

        private SeoContentDTO MapToDto(SeoContent seo) => new()
        {
            Id = seo.Id,
            Route = seo.Route,
            Language = seo.Language,
            Title = seo.Title,
            Description = seo.Description,
            Keywords = seo.Keywords,
            OgTitle = seo.OgTitle,
            OgDescription = seo.OgDescription,
            OgImage = seo.OgImage,
            CanonicalUrl = seo.CanonicalUrl,
            Robots = seo.Robots,
            CreatedAt = seo.CreatedAt,
            UpdatedAt = seo.UpdatedAt
        };
    }
}
