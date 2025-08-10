using Core.DTOs;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class TechService : ITechService
    {
        private readonly PortfolioDbContext context;
        private readonly MediaUploadService mediaUploadService;

        public TechService(PortfolioDbContext portfolio, MediaUploadService media)
        {
            context = portfolio;
            mediaUploadService = media;
        }

        public async Task<TechnologyDTO> CreateAsync(CreateTechnologyDTO dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            string? uploadedPath = null;
            await using var tx = await context.Database.BeginTransactionAsync();
            try
            {
                string? iconUrl = null;
                if (dto.IconFile != null)
                {
                    var res = await mediaUploadService.UploadImageWithPath(dto.IconFile, dto.Name);
                    uploadedPath = res.PhysicalPath;
                    iconUrl = res.Url;
                }

                var tech = new Technology
                {
                    Name = dto.Name,
                    IconUrl = iconUrl
                };

                context.Add(tech);
                await context.SaveChangesAsync();
                await tx.CommitAsync();

                return MapToTechnologyDTO(tech);
            }
            catch
            {
                await tx.RollbackAsync();
                if (!string.IsNullOrWhiteSpace(uploadedPath))
                {
                    try { await mediaUploadService.DeleteIfExistsAsync(uploadedPath); } catch { }
                }
                throw;
            }
        }

        public async Task<List<TechnologyDTO>> GetAllAsync()
        {
            var tech = await context.Technologies
                .AsNoTracking()
                .ToListAsync();

            return tech.Select(MapToTechnologyDTO).ToList();
        }

        private static TechnologyDTO MapToTechnologyDTO(Technology t) => new()
        {
            Id = t.Id,
            Name = t.Name,
            IconUrl = t.IconUrl
        };

        public async Task<TechnologyDTO> UpdateAsync(UpdateTechDTO dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            var technology = await context.Technologies
                .FirstOrDefaultAsync(t => t.Id == dto.Id);

            if (technology == null) throw new ArgumentException("Technology not found");

            string? uploadedPath = null;
            await using var tx = await context.Database.BeginTransactionAsync();

            try
            {
                // Handle icon update
                if (dto.IconFile != null)
                {
                    var res = await mediaUploadService.UploadImageWithPath(dto.IconFile, dto.Name);
                    uploadedPath = res.PhysicalPath;
                    technology.IconUrl = res.Url;
                }

                // Update other properties
                technology.Name = dto.Name;

                context.Update(technology);
                await context.SaveChangesAsync();
                await tx.CommitAsync();

                return MapToTechnologyDTO(technology);
            }
            catch
            {
                // rollback db and clean uploaded files
                await tx.RollbackAsync();
                if (!string.IsNullOrWhiteSpace(uploadedPath))
                {
                    try { await mediaUploadService.DeleteIfExistsAsync(uploadedPath); } catch { /* swallow */ }
                }
                throw;
            }
        }
    }
}