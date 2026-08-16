using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Infrastructure.Data;
using Application.Interfaces;
using Application.DTOs.Seo;

namespace AdminDashboard.Controllers
{
    public class SeoContentsController : Controller
    {
        private readonly PortfolioDbContext _context;
        private readonly ISeoService _seoService;

        public SeoContentsController(PortfolioDbContext context, ISeoService seoService)
        {
            _context = context;
            _seoService = seoService;
        }

        // GET: SeoContents
        public async Task<IActionResult> Index()
        {
            return View(await _context.SeoContents.ToListAsync());
        }

        // GET: SeoContents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seoContent = await _context.SeoContents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seoContent == null)
            {
                return NotFound();
            }

            return View(seoContent);
        }

        // GET: SeoContents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SeoContents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateSeoContentDTO seo)
        {
            if (ModelState.IsValid)
            {
                var result = _seoService.CreateSeoContent(seo);
                //_context.Add(seoContent);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(seo);
        }
        
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Route,Language,Title,Description,Keywords,OgTitle,OgDescription,OgImage,CanonicalUrl,Robots,CreatedAt,UpdatedAt")] SeoContent seoContent)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(seoContent);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(seoContent);
        //}

        // GET: SeoContents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seoContent = await _context.SeoContents.FindAsync(id);
            if (seoContent == null)
            {
                return NotFound();
            }
            return View(seoContent);
        }

        // POST: SeoContents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Route,Language,Title,Description,Keywords,OgTitle,OgDescription,OgImage,CanonicalUrl,Robots,CreatedAt,UpdatedAt")] SeoContent seoContent)
        {
            if (id != seoContent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seoContent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeoContentExists(seoContent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(seoContent);
        }

        // GET: SeoContents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seoContent = await _context.SeoContents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seoContent == null)
            {
                return NotFound();
            }

            return View(seoContent);
        }

        // POST: SeoContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seoContent = await _context.SeoContents.FindAsync(id);
            if (seoContent != null)
            {
                _context.SeoContents.Remove(seoContent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeoContentExists(int id)
        {
            return _context.SeoContents.Any(e => e.Id == id);
        }
    }
}
