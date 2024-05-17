using LaptopShop2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace LaptopShop2.Controllers
{
    public class HomeController : Controller
    {
        private readonly LaptopShopContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, LaptopShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.TbProducts
                           .Include(p => p.CategoryProduct)
                           .Where(m => m.IsActive)
                           .OrderByDescending(i => i.ProductId)
                           .ToList();

            var categories = _context.TbCategoryProducts.ToList();

            ViewBag.Products = products;
            ViewBag.Categories = categories;

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
