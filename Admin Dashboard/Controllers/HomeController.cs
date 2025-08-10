using Admin_Dashboard.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Admin_Dashboard.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PortfolioDbContext context;

        public HomeController(ILogger<HomeController> logger,
            PortfolioDbContext portfolio
            )
        {
            context = portfolio;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.ProjectCount = context.Projects.Count();
            ViewBag.TechnologiesCount = context.Technologies.Count();
            ViewBag.EntriesCount = context.ContactEntries.Count();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
