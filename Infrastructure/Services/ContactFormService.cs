using Core.DTOs;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;

namespace Infrastructure.Services
{
    public class ContactFormService : IContactFormService
    {
        private readonly PortfolioDbContext context;
        public ContactFormService(PortfolioDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> AddNewEntry(ContactDTO newEntry)
        {
            if (newEntry == null)
                throw new Exception();

            var entry = new ContactForm
            {
                FullName = newEntry.FullName,
                PhoneNumber = newEntry.PhoneNumber,
                Email = newEntry.Email,
                Message = newEntry.Message,
                TimeStamp = newEntry.TimeStamp
            };

            await context.AddAsync(entry);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
