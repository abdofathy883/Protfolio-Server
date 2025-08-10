using Core.DTOs;

namespace Core.Interfaces
{
    public interface IContactFormService
    {
        Task<bool> AddNewEntry(ContactDTO newEntry);
    }
}
