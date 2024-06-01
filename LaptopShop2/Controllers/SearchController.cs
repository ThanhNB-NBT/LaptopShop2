using LaptopShop2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaptopShop2.Controllers
{
    public class SearchController : Controller
    {
        private readonly LaptopShopContext _context;
        public SearchController(LaptopShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ResultsAsync(string query)
        {
            var results = _context.TbProducts.Where(p => p.ProductName.Contains(query)).ToList();
            ViewBag.SearchString = query;
            return View("Results", results);
        }

        public IActionResult ProductBrands(int brandId)
        {
            var products = _context.TbProducts.Where(p => p.BrandId == brandId).ToList();
            var brand = _context.TbBrands.Find(brandId);
            if (brand == null)
            {
                return NotFound();
            }

            ViewBag.BrandName = brand.Name;
            ViewBag.HasProducts = products.Any(); // Biến kiểm tra

            return View(products);
        }
    }
}
