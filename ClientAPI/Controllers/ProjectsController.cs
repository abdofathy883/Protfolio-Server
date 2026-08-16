using Application.Interfaces;
using Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ClientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectPublicService _projectService;
        public ProjectsController(IProjectPublicService service)
        {
            _projectService = service;
        }

        [HttpGet("{lang}")]
        public async Task<IActionResult> GetAllAsync(Language lang)
        {
            var projects = await _projectService.GetAll(lang);

            if (projects is null)
                return NotFound();

            return Ok(projects);
        }

        [HttpGet("{slug}/{lang}")]
        public async Task<IActionResult> GetBySlug(string slug, Language lang)
        {
            var project = await _projectService.GetBySlug(slug, lang);

            if (project is null)
                return NotFound();

            return Ok(project);
        }

        [HttpGet("slugs")]
        public async Task<IActionResult> GetSlugs()
        {
            var slugs = await _projectService.GetAllSlugs();
            return Ok(slugs);
        }
    }
}
