using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ClientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        private readonly IContactFormService contactService;
        public ContactFormController(IContactFormService service)
        {
            contactService = service;
        }

        [EnableRateLimiting("contact-limit")]
        [HttpPost]
        public async Task<IActionResult> Submit(ContactDTO newEntryDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!string.IsNullOrEmpty(newEntryDTO.Website))
                return BadRequest("Spam Detected");

            var isHuman = await contactService.VerifyTokenAsync(newEntryDTO.RecaptchaToken);

            if (!isHuman)
                return BadRequest("Spam Detected");

            var result = await contactService.AddNewEntry(newEntryDTO);
            return Ok(result);
        }
    }
}
