using Core.DTOs;
using Core.DTOs.Projects;
using Core.Enums;

namespace Core.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectDTO>> GetAll(Language lang);
        //Task<ProjectDTO> UpdateAsync(UpdateProjectDTO dto);

        Task<ProjectDTO> Create(CreateProjectDTO project, int? itemId = null);
        Task<ProjectDTO> GetById(int id);
        Task<ProjectDTO> GetBySlug(string slug, Language lang);
    }
}
