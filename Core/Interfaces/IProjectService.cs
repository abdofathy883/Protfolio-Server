using Core.DTOs;
using Core.Enums;

namespace Core.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectDTO>> GetAll(Language lang);
        Task<ProjectDTO> GetById(int id, Language lang);
        Task<ProjectDTO> CreateAsync(CreateProjectDTO projectDTO);
        Task<ProjectDTO> UpdateAsync(UpdateProjectDTO dto);
    }
}
