using Core.DTOs;
using Core.DTOs.Projects;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly PortfolioDbContext context;
        private readonly MediaUploadService mediaUploadService;
        public ProjectService(
            PortfolioDbContext dbContext,
            MediaUploadService service
            )
        {
            context = dbContext;
            mediaUploadService = service;
        }
        public async Task<List<ProjectDTO>> GetAll(Language lang)
        {
            var projects = await context.Projects
                .Where(p => p.Translations.Any(t => t.Language == lang))
                .Include(p => p.Translations.Where(t => t.Language == lang))
                .ToListAsync();

            return projects.Select(MapToProjectDTO).ToList();
        }

        public async Task<ProjectDTO> GetById(int id)
        {
            var project = await context.Projects
                .Include(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new Exception();

            return MapToProjectDTO(project);
        }

        public async Task<ProjectDTO> Create(CreateProjectDTO dto, int? itemId)
        {
            Project item;

            if (itemId.HasValue)
            {
                item = await context.Projects
                    .Include(p => p.Translations)
                    .FirstOrDefaultAsync(p => p.Id == itemId.Value)
                    ?? throw new KeyNotFoundException($"No portfolio item found with id {itemId}");

                // Add translations
                foreach (var t in dto.Translations)
                {
                    if (!item.Translations.Any(tr => tr.Language == t.Language))
                    {
                        item.Translations.Add(new ProjectTranslation
                        {
                            Language = t.Language,
                            Title = t.Title,
                            Description = t.Description,
                            Client = t.Client,
                            Problem = t.Problem,
                            Solution = t.Solution,
                            VideoAltText = t.VideoAltText,
                            ImageAltText = t.ImageAltText
                        });
                    }
                }
            }
            else
            {
                var featuredImagURL = string.Empty;
                var videoUrl = string.Empty;
                if (dto.ImageFile is not null)
                {
                    var uploaded = await mediaUploadService.UploadImageWithPath(
                        dto.ImageFile,
                        $"{dto.Slug}"
                        );
                    featuredImagURL = uploaded.Url;
                }

                if (dto.VideoFile is not null)
                {
                    var uploaded = await mediaUploadService.UploadVideoWithPath(dto.VideoFile, $"{dto.Slug}");
                    videoUrl = uploaded.Url;
                }

                item = new Project
                {
                    Slug = CreateSlug(dto.Slug ?? dto.Translations.First().Title),
                    ImageLink = featuredImagURL,
                    VideoLink = videoUrl,
                    PublishedAt = DateTime.UtcNow,
                    
                    Translations = dto.Translations.Select(t => new ProjectTranslation
                    {
                        Title = t.Title,
                        Description = t.Description,
                        Language = t.Language,
                        Client = t.Client,
                        Problem = t.Problem,
                        Solution = t.Solution,
                        VideoAltText = t.VideoAltText,
                        ImageAltText = t.ImageAltText
                    }).ToList()
                };

                if (dto.Technologies != null && dto.Technologies.Any())
                {
                    var techEntities = await context.Technologies
                        .Where(t => dto.Technologies.Contains(t.Name))
                        .ToListAsync();

                    foreach (var tech in techEntities)
                    {
                        item.Technologies.Add(tech);
                    }
                }
                await context.Projects.AddAsync(item);
            }

            await context.SaveChangesAsync();
            return MapToProjectDTO(item);
        }

        private ProjectDTO MapToProjectDTO(Project p) => new()
        {
            Id = p.Id,
            Slug = p.Slug,
            PublishedAt = p.PublishedAt,
            ImageLink = p.ImageLink,
            VideoLink = p.VideoLink,
            Technologies = p.Technologies?.Select(t => new TechnologyDTO
            {
                Id = t.Id,
                Name = t.Name,
                IconUrl = t.IconUrl
            }).ToList() ?? new List<TechnologyDTO>(),
            Translations = p.Translations?.Select(t => new ProjectTranslationDto
            {
                Id = t.Id,
                ProjectID = t.ProjectID,
                Language = t.Language,
                Title = t.Title,
                Description = t.Description,
                Client = t.Client,
                Problem = t.Problem,
                Solution = t.Solution
            }).ToList() ?? new List<ProjectTranslationDto>(),
            LiveUrl = p.LiveUrl,
            DemoUrl = p.DemoUrl
        };

        public async Task<ProjectDTO> GetBySlug(string slug, Language lang)
        {
            var project = await context.Projects
                .Where(p => p.Translations.Any(t => t.Language == lang))
                .Include(p => p.Translations.Where(t => t.Language == lang))
                .FirstOrDefaultAsync(p => p.Slug == slug)
                ?? throw new Exception();
            return MapToProjectDTO(project);
        }

        private string CreateSlug(string title)
        {
            var slug = string.Join("-", title.ToLower().Split(' ', '-'));
            //var slug = title.ToLower().Split(' ', '-').ToString();
            return slug;
        }

        //public async Task<ProjectDTO> UpdateAsync(UpdateProjectDTO dto)
        //{
        //    if (dto is null) throw new ArgumentNullException(nameof(dto));

        //    var project = await context.Projects
        //        .Include(p => p.Images)
        //        .FirstOrDefaultAsync(p => p.Id == dto.Id);

        //    if (project == null) throw new ArgumentException("Project not found");

        //    var uploadedPaths = new List<string>();
        //    await using var tx = await context.Database.BeginTransactionAsync();

        //    try
        //    {
        //        // Handle video update
        //        if (dto.VideoFile != null)
        //        {
        //            var res = await mediaUploadService.UploadVideoWithPath(dto.VideoFile, dto.Title);
        //            uploadedPaths.Add(res.PhysicalPath);
        //            project.Video = res.Url;
        //        }

        //        // Handle image updates
        //        var imagesToDelete = new List<ProjectImage>();
        //        var imagesToAdd = new List<ProjectImage>();
        //        var imagesToUpdate = new List<ProjectImage>();

        //        foreach (var imgDto in dto.Images)
        //        {
        //            if (imgDto.ToDelete)
        //            {
        //                // Mark existing image for deletion
        //                var existingImg = project.Images.FirstOrDefault(i => i.Id == imgDto.Id);
        //                if (existingImg != null)
        //                {
        //                    imagesToDelete.Add(existingImg);
        //                }
        //            }
        //            else if (imgDto.Id == 0 && imgDto.ImageFile != null)
        //            {
        //                // New image
        //                var res = await mediaUploadService.UploadImageWithPath(imgDto.ImageFile, dto.Title);
        //                uploadedPaths.Add(res.PhysicalPath);

        //                imagesToAdd.Add(new ProjectImage
        //                {
        //                    Url = res.Url,
        //                    AltText = imgDto.AltText,
        //                    IsFeatured = imgDto.IsFeatured,
        //                    ProjectId = project.Id
        //                });
        //            }
        //            else if (imgDto.Id > 0)
        //            {
        //                // Update existing image
        //                var existingImg = project.Images.FirstOrDefault(i => i.Id == imgDto.Id);
        //                if (existingImg != null)
        //                {
        //                    existingImg.AltText = imgDto.AltText;
        //                    existingImg.IsFeatured = imgDto.IsFeatured;

        //                    // Handle image file update if provided
        //                    if (imgDto.ImageFile != null)
        //                    {
        //                        var res = await mediaUploadService.UploadImageWithPath(imgDto.ImageFile, dto.Title);
        //                        uploadedPaths.Add(res.PhysicalPath);
        //                        existingImg.Url = res.Url;
        //                    }
        //                }
        //            }
        //        }

        //        // Remove deleted images
        //        foreach (var img in imagesToDelete)
        //        {
        //            project.Images.Remove(img);
        //            context.ProjectImages.Remove(img);
        //        }

        //        // Add new images
        //        foreach (var img in imagesToAdd)
        //        {
        //            project.Images.Add(img);
        //        }

        //        // Update other properties
        //        project.Title = dto.Title;
        //        project.Description = dto.Description;
        //        project.Technologies = dto.Technologies;
        //        project.Client = dto.Client;
        //        project.Problem = dto.Problem;
        //        project.Solution = dto.Solution;
        //        project.LiveUrl = dto.LiveUrl;
        //        project.DemoUrl = dto.DemoUrl;
        //        project.IsFeatured = dto.IsFeatured;

        //        context.Update(project);
        //        await context.SaveChangesAsync();
        //        await tx.CommitAsync();

        //        return MapToProjectDTO(project);
        //    }
        //    catch
        //    {
        //        // rollback db and clean uploaded files
        //        await tx.RollbackAsync();
        //        foreach (var p in uploadedPaths.Distinct())
        //        {
        //            try { await mediaUploadService.DeleteIfExistsAsync(p); } catch { /* swallow */ }
        //        }
        //        throw;
        //    }
        //}
    }
}
