using Core.DTOs;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
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
                .AsNoTracking()
                .Include(p => p.Images)
                .Where(p => p.Language == lang)
                .ToListAsync()
                ?? throw new Exception();

            return projects.Select(MapToProjectDTO).ToList();
        }

        public async Task<ProjectDTO> GetById(int id, Language lang)
        {
            var project = await context.Projects
                .AsNoTracking()
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id && p.Language == lang)
                ?? throw new Exception();

            return MapToProjectDTO(project);
        }

        public async Task<ProjectDTO> CreateAsync(CreateProjectDTO dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            var uploadedPaths = new List<string>();
            await using var tx = await context.Database.BeginTransactionAsync();
            try
            {
                // ✅ Step 1: find existing project by ContentId
                

                int projectId;
                if (dto.ProjectID == 0) // new project
                {
                    projectId = await context.Projects.AnyAsync()
                        ? await context.Projects.MaxAsync(p => p.ProjectID) + 1
                        : 1;
                }
                else
                {
                    projectId = dto.ProjectID;
                }

                var imageResults = new List<(string Url, string? Alt, bool IsFeatured, string Path)>();
                foreach (var img in dto.Images)
                {
                    var res = await mediaUploadService.UploadImageWithPath(img.ImageFile, dto.Title);
                    uploadedPaths.Add(res.PhysicalPath);
                    imageResults.Add((res.Url, img.AltText, img.IsFeatured, res.PhysicalPath));
                }

                string? videoUrl = null;
                string? videoPath = null;
                if (dto.VideoFile != null)
                {
                    var res = await mediaUploadService.UploadVideoWithPath(dto.VideoFile, dto.Title);
                    uploadedPaths.Add(res.PhysicalPath);
                    videoUrl = res.Url;
                    videoPath = res.PhysicalPath;
                }

               

                var project = new Project
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    Language = dto.Language,
                    ProjectID = projectId,
                    Images = imageResults.Select(x => new ProjectImage
                    {
                        Url = x.Url,
                        AltText = x.Alt,
                        IsFeatured = x.IsFeatured
                    }).ToList(),
                    Video = videoUrl,
                    Technologies = dto.Technologies,
                    Client = dto.Client,
                    Problem = dto.Problem,
                    Solution = dto.Solution,
                    LiveUrl = dto.LiveUrl,
                    DemoUrl = dto.DemoUrl,
                    IsFeatured = dto.IsFeatured
                };

                context.Add(project);
                await context.SaveChangesAsync();
                await tx.CommitAsync();

                return MapToProjectDTO(project);
            }
            catch
            {
                // rollback db and clean uploaded files
                await tx.RollbackAsync();
                foreach (var p in uploadedPaths.Distinct())
                {
                    try { await mediaUploadService.DeleteIfExistsAsync(p); } catch { /* swallow */ }
                }
                throw;
            }
        }

        private static ProjectImageDTO MapToImageDTO(ProjectImage img) => new()
        {
            Id = img.Id,
            Url = img.Url,
            AltText = img.AltText,
            IsFeatured = img.IsFeatured,
            ProjectId = img.ProjectId
        };

        private ProjectDTO MapToProjectDTO(Project p) => new()
        {
            Id = p.Id,
            Title = p.Title,
            Language = p.Language,
            ProjectId = p.ProjectID,
            Description = p.Description,
            Images = p.Images.Select(MapToImageDTO).ToList(),
            Video = p.Video,
            Technologies = p.Technologies,
            Client = p.Client,
            Problem = p.Problem,
            Solution = p.Solution,
            LiveUrl = p.LiveUrl,
            DemoUrl = p.DemoUrl,
            IsFeatured = p.IsFeatured
        };

        public async Task<ProjectDTO> UpdateAsync(UpdateProjectDTO dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            var project = await context.Projects
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == dto.Id);

            if (project == null) throw new ArgumentException("Project not found");

            var uploadedPaths = new List<string>();
            await using var tx = await context.Database.BeginTransactionAsync();

            try
            {
                // Handle video update
                if (dto.VideoFile != null)
                {
                    var res = await mediaUploadService.UploadVideoWithPath(dto.VideoFile, dto.Title);
                    uploadedPaths.Add(res.PhysicalPath);
                    project.Video = res.Url;
                }

                // Handle image updates
                var imagesToDelete = new List<ProjectImage>();
                var imagesToAdd = new List<ProjectImage>();
                var imagesToUpdate = new List<ProjectImage>();

                foreach (var imgDto in dto.Images)
                {
                    if (imgDto.ToDelete)
                    {
                        // Mark existing image for deletion
                        var existingImg = project.Images.FirstOrDefault(i => i.Id == imgDto.Id);
                        if (existingImg != null)
                        {
                            imagesToDelete.Add(existingImg);
                        }
                    }
                    else if (imgDto.Id == 0 && imgDto.ImageFile != null)
                    {
                        // New image
                        var res = await mediaUploadService.UploadImageWithPath(imgDto.ImageFile, dto.Title);
                        uploadedPaths.Add(res.PhysicalPath);

                        imagesToAdd.Add(new ProjectImage
                        {
                            Url = res.Url,
                            AltText = imgDto.AltText,
                            IsFeatured = imgDto.IsFeatured,
                            ProjectId = project.Id
                        });
                    }
                    else if (imgDto.Id > 0)
                    {
                        // Update existing image
                        var existingImg = project.Images.FirstOrDefault(i => i.Id == imgDto.Id);
                        if (existingImg != null)
                        {
                            existingImg.AltText = imgDto.AltText;
                            existingImg.IsFeatured = imgDto.IsFeatured;

                            // Handle image file update if provided
                            if (imgDto.ImageFile != null)
                            {
                                var res = await mediaUploadService.UploadImageWithPath(imgDto.ImageFile, dto.Title);
                                uploadedPaths.Add(res.PhysicalPath);
                                existingImg.Url = res.Url;
                            }
                        }
                    }
                }

                // Remove deleted images
                foreach (var img in imagesToDelete)
                {
                    project.Images.Remove(img);
                    context.ProjectImages.Remove(img);
                }

                // Add new images
                foreach (var img in imagesToAdd)
                {
                    project.Images.Add(img);
                }

                // Update other properties
                project.Title = dto.Title;
                project.Description = dto.Description;
                project.Technologies = dto.Technologies;
                project.Client = dto.Client;
                project.Problem = dto.Problem;
                project.Solution = dto.Solution;
                project.LiveUrl = dto.LiveUrl;
                project.DemoUrl = dto.DemoUrl;
                project.IsFeatured = dto.IsFeatured;

                context.Update(project);
                await context.SaveChangesAsync();
                await tx.CommitAsync();

                return MapToProjectDTO(project);
            }
            catch
            {
                // rollback db and clean uploaded files
                await tx.RollbackAsync();
                foreach (var p in uploadedPaths.Distinct())
                {
                    try { await mediaUploadService.DeleteIfExistsAsync(p); } catch { /* swallow */ }
                }
                throw;
            }
        }
    }
}
