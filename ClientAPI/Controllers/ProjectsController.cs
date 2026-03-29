using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService projectService;
        public ProjectsController(IProjectService service)
        {
            projectService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(Language lang)
        {
            var projects = await projectService.GetAll(lang);

            if (projects is null)
                return NotFound();

            return Ok(projects);
        }

        [HttpGet("{slug}/{lang}")]
        public async Task<IActionResult> GetBySlug(string slug, Language lang)
        {
            var project = await projectService.GetBySlug(slug, lang);

            if (project is null)
                return NotFound();

            return Ok(project);
        }
    }
}
