using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client_API.Controllers
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
        public async Task<IActionResult> GetAllAsync()
        {
            var projects = await projectService.GetAll();

            if (projects is null)
                return NotFound();

            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await projectService.GetById(id);

            if (project is null)
                return NotFound();

            return Ok(project);
        }
    }
}
