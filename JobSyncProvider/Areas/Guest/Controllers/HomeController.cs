using JobSyncProvider.DataAccess.Services;
using JobSyncProvider.Models;
using JobSyncProvider.Utilities;
using JobSyncProvider.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace JobSyncProvider.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService _categoryService;
        public HomeController(ILogger<HomeController> logger,ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            var homeVW = new HomeVM
            {
                Categories = categories.Where(c => c.Status == StaticDetails.STATUS_VALID).Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.CategoryId.ToString(),
                }).OrderBy(c => c.Text)
            };

            return View(homeVW);
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
