using Infrastructure.Services;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers
{
    [Authorize]
    public class LaTexEngineController : Controller
    {
        private readonly LaTexEngine latexEngine;

        public LaTexEngineController(LaTexEngine _latexEngine)
        {
            latexEngine = _latexEngine;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ProposalVM());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProposalVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var texContent = await latexEngine.RenderLatexAsync(vm);

            // Just return .tex as a download for now
            var bytes = System.Text.Encoding.UTF8.GetBytes(texContent);
            return File(bytes, "application/x-tex", "proposal.tex");
        }
    }
}
