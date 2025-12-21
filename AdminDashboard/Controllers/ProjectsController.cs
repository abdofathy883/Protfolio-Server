
using Core.DTOs;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly PortfolioDbContext _context;
        private readonly IProjectService projectService;

        public ProjectsController(
            PortfolioDbContext context,
            IProjectService service
            )
        {
            _context = context;
            projectService = service;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            var techNames = _context.Technologies
                .AsNoTracking()
                .Select(t => t.Name)
                .ToList();

            ViewBag.Technologies = techNames
                .Select(n => new SelectListItem { Value = n, Text = n })
                .ToList();

            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateProjectDTO project)
        {
            if (!ModelState.IsValid)
            {
                var techNames = await _context.Technologies
                    .AsNoTracking()
                    .Select(t => t.Name)
                    .ToListAsync();

                ViewBag.Technologies = techNames
                    .Select(n => new SelectListItem { Value = n, Text = n })
                    .ToList();

                return View(project);
            }

            var created = await projectService.CreateAsync(project);
            return RedirectToAction(nameof(Index));

        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Title,Description,Video,Technologies,Client,Problem,Solution,LiveUrl,DemoUrl,IsFeatured")] Project project)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(project);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(project);
        //}

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var techNames = _context.Technologies
                .AsNoTracking()
                .Select(t => t.Name)
                .ToList();

            ViewBag.Technologies = techNames
                .Select(n => new SelectListItem { Value = n, Text = n })
                .ToList();

            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] UpdateProjectDTO project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var updated = await projectService.UpdateAsync(project);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the project: " + ex.Message);

                    var techNames = await _context.Technologies
                        .AsNoTracking()
                        .Select(t => t.Name)
                        .ToListAsync();

                    ViewBag.Technologies = techNames
                        .Select(n => new SelectListItem { Value = n, Text = n })
                        .ToList();

                    return View(project);
                }
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
