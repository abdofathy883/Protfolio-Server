using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Core.Interfaces;
using Core.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace AdminDashboard.Controllers
{
    [Authorize]
    public class TechnologiesController : Controller
    {
        private readonly PortfolioDbContext _context;
        private readonly ITechService techService;

        public TechnologiesController(PortfolioDbContext context, 
            ITechService tech)
        {
            _context = context;
            techService = tech;
        }

        // GET: Technologies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Technologies.ToListAsync());
        }

        // GET: Technologies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Technologies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateTechnologyDTO technology)
        {
            if (ModelState.IsValid)
            {
                await techService.CreateAsync(technology);
                return RedirectToAction(nameof(Index));
            }
            return View(technology);
        }

        // GET: Technologies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technology = await _context.Technologies.FindAsync(id);
            if (technology == null)
            {
                return NotFound();
            }
            return View(technology);
        }

        // POST: Technologies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] UpdateTechDTO technology)
        {
            if (id != technology.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var updated = await techService.UpdateAsync(technology);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the technology: " + ex.Message);
                    return View(technology);
                }
            }
            return View(technology);
        }

        // GET: Technologies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technology = await _context.Technologies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (technology == null)
            {
                return NotFound();
            }

            return View(technology);
        }

        // POST: Technologies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var technology = await _context.Technologies.FindAsync(id);
            if (technology != null)
            {
                _context.Technologies.Remove(technology);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TechnologyExists(int id)
        {
            return _context.Technologies.Any(e => e.Id == id);
        }
    }
}
