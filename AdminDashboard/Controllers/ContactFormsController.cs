using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;

namespace AdminDashboard.Controllers
{
    [Authorize]
    public class ContactFormsController : Controller
    {
        private readonly PortfolioDbContext _context;

        public ContactFormsController(PortfolioDbContext context)
        {
            _context = context;
        }

        // GET: ContactForms
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContactEntries.ToListAsync());
        }

        // GET: ContactForms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactForm = await _context.ContactEntries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactForm == null)
            {
                return NotFound();
            }

            return View(contactForm);
        }

        // GET: ContactForms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactForm = await _context.ContactEntries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactForm == null)
            {
                return NotFound();
            }

            return View(contactForm);
        }

        // POST: ContactForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactForm = await _context.ContactEntries.FindAsync(id);
            if (contactForm != null)
            {
                _context.ContactEntries.Remove(contactForm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
