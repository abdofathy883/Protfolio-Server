using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        private readonly IContactFormService formService;
        public ContactFormController(IContactFormService service)
        {
            formService = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddNewEntry(ContactDTO contactDTO)
        {
            if (contactDTO is null)
                return BadRequest();

            var result = await formService.AddNewEntry(contactDTO);
            
            return Ok(result);
        } 
    }
}
