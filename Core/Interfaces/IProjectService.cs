using Core.DTOs;

namespace Core.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectDTO>> GetAll();
        Task<ProjectDTO> GetById(int id);
        Task<ProjectDTO> CreateAsync(CreateProjectDTO projectDTO);
        Task<ProjectDTO> UpdateAsync(UpdateProjectDTO dto);
    }
}
