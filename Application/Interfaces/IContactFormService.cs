using Application.DTOs;

namespace Application.Interfaces
{
    public interface IContactFormService
    {
        Task<bool> AddNewEntry(ContactDTO newEntry);
        Task<bool> VerifyTokenAsync(string token);
    }
}
