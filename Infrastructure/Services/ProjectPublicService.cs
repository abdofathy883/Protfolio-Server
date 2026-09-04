using Application.DTOs.Projects;
using Application.Interfaces;
using Core.Entities;
using Core.Enums;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace Infrastructure.Services
{
    public class ProjectPublicService : IProjectPublicService
    {
        private readonly PortfolioDbContext _dbContext;

        public ProjectPublicService(PortfolioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ProjectPublicDto>> GetAll(Language lang)
        {
            IQueryable<Project> query = _dbContext.Projects
                .AsNoTracking()
                .Where(p => p.Translations.Any());

            var result = await query
                .OrderByDescending(p => p.PublishedAt)
                .Select(p => new
                {
                    Project = p,
                    Translations = p.Translations
                    .FirstOrDefault(t => t.Language == lang)
                })
                .Where(b => b.Translations != null)
                .Select(p => new ProjectPublicDto
                {
                    Id = p.Project.Id,
                    Slug = p.Translations.Slug,
                    ImageLink = p.Project.ImageLink,
                    ImageAltText = p.Translations.ImageAltText,
                    VideoLink = p.Project.VideoLink,
                    VideoAltText = p.Translations.VideoAltText,
                    Title = p.Translations.Title,
                    Excerpt = p.Translations.Excerpt,
                    Description = p.Translations.Description,
                    PublishedAt = p.Project.PublishedAt,
                    Problem = p.Translations.Problem,
                    Solution = p.Translations.Solution
                })
                .ToListAsync();

            return result;
        }

        public async Task<List<string>> GetAllSlugs()
        {
            return await _dbContext.ProjectTranslations
                .AsNoTracking()
                .Where(p => p.Slug != null)
                .Select(p => p.Slug!)
                .ToListAsync();
        }

        public async Task<ProjectPublicDto> GetBySlug(string slug, Language lang)
        {
            var item = await _dbContext.Projects
                .AsNoTracking()
                .Include(pi => pi.Translations)
                .Where(pi => pi.Translations.Any(t => t.Slug == slug))
                .SingleAsync();

            var translation = item.Translations.First(t => t.Language == lang);

            return new ProjectPublicDto
            {
                Id = item.Id,
                Slug = translation.Slug,
                ImageLink = item.ImageLink,
                ImageAltText = translation.ImageAltText,
                VideoLink = item.VideoLink,
                VideoAltText = translation.VideoAltText,
                Title = translation.Title,
                Excerpt = translation.Excerpt,
                Description = translation.Description,
                PublishedAt = item.PublishedAt,
                Problem = translation.Problem,
                Solution = translation.Solution,
                Seo = translation.Seo == null
                ? null : new Application.Dtos.Seo.SeoMetaDto
                {
                    MetaTitle = translation.Seo.MetaTitle,
                    MetaDescription = translation.Seo.MetaDescription,
                    OgTitle = translation.Seo.OgTitle,
                    OgDescription = translation.Seo.OgDescription,
                    OgImageUrl = translation.Seo.OgImageUrl,
                    Keywords = translation.Seo.Keywords,
                    NoFollow = translation.Seo.NoFollow,
                    NoIndex = translation.Seo.NoIndex
                }
            };
        }
    }
}
