using Microsoft.AspNetCore.Mvc;
using Core.Enums;
using Application.Interfaces;
using Application.DTOs.Seo;

namespace ClientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeoController : ControllerBase
    {
        private readonly ISeoService seoService;

        public SeoController(ISeoService seoService)
        {
            this.seoService = seoService;
        }

        [HttpGet("{language}/{*route}")]
        public async Task<IActionResult> Get(Language language, string route)
        {
            var content = await seoService.GetContentByRoute(route, language);
            if (content == null)
            {
                 return Ok(new SeoContentDTO { Route = route, Language = language });
            }
            return Ok(content);
        }

        //[HttpPost]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> Save([FromForm] CreateSeoContentDTO model)
        //{
        //    if (string.IsNullOrEmpty(model.Route))
        //    {
        //         return BadRequest("Route is required.");
        //    }

        //    var result = await seoService.UpdateSeoContent(model);
        //    return Ok(result);
        //}
    }
}
