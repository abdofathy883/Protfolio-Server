using Application.DTOs.Technologies;

namespace Application.Interfaces
{
    public interface ITechService
    {
        Task<TechnologyDTO> CreateAsync(CreateTechnologyDTO dto);
        Task<List<TechnologyDTO>> GetAllAsync();
        Task<TechnologyDTO> UpdateAsync(UpdateTechDTO dto);
    }
}
