using Application.DTOs.Projects;
using Core.Enums;

namespace Application.Interfaces
{
    public interface IProjectPublicService
    {
        Task<List<ProjectPublicDto>> GetAll(Language lang);
        Task<ProjectPublicDto> GetBySlug(string slug, Language lang);
        Task<List<string>> GetAllSlugs();
    }
}
