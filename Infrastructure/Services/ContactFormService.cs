using Core.DTOs;
using Core.DTOs.ContactForm;
using Core.Interfaces;
using Core.Models;
using Core.Settings;
using Infrastructure.Data;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class ContactFormService : IContactFormService
    {
        private readonly PortfolioDbContext context;
        private readonly HttpClient httpClient;
        private readonly IOptions<RecaptchaSeetings> options;

        public ContactFormService(PortfolioDbContext context, HttpClient httpClient, IOptions<RecaptchaSeetings> options)
        {
            this.context = context;
            this.httpClient = httpClient;
            this.options = options;
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

        public async Task<bool> VerifyTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            var secretKey = options.Value.SecretKey;
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("secret", secretKey),
                new KeyValuePair<string, string>("response", token)
            });

            var response = await httpClient.PostAsync(
                "https://www.google.com/recaptcha/api/siteverify",
                content);

            if (!response.IsSuccessStatusCode)
                return false;

            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine(json);

            var result = JsonSerializer.Deserialize<RecaptchaResponse>(json);

            if (result is null)
                return false;

            return result.Success;
        }
    }
}
