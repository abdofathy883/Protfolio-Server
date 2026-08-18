using Application.DTOs.Seo;
using Core.Enums;

namespace Application.Interfaces
{
    public interface ISeoService
    {
        Task<SeoContentDTO> GetContentByRoute(string route, Language language);
        Task<SeoContentDTO> CreateSeoContent(CreateSeoContentDTO newContent);
        Task<SeoContentDTO> UpdateSeoContent(UpdateSeoDto content);
    }
}
