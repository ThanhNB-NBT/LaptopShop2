using LaptopShop2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopShop2.Functions;
using LaptopShop2.ViewModels;

namespace LaptopShop2.Controllers
{
    public class ProductController : Controller
    {
        private readonly LaptopShopContext _context;
        public ProductController(LaptopShopContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? categoryId)
        {
            var products = _context.TbProducts.Include(p => p.Brand).Include(p => p.CategoryProduct).ToList();

            var brand = _context.TbBrands.ToList();
            ViewBag.Brand = brand;

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryProductId == categoryId).ToList();
            }
            ViewBag.Categories = _context.TbCategoryProducts.ToList();
            ViewBag.SelectedCategoryId = categoryId;
            // Lấy các tùy chọn lọc và gán chúng vào ViewBag hoặc ViewData
            ViewBag.FilterOptions = GetFilterOptions();
            return View(products);
        }


        [Route("/product-{slug}-{id:}.html", Name = "Detail")]
        public IActionResult Detail(int? id)
        {
            if(id == null)
            {
                NotFound();
            }

            var product = _context.TbProducts.Include(p => p.Brand)
                                            .Include(p => p.TbImages)
                                            .Include(p => p.CategoryProduct)
                                            .FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            // Retrieve related products
            var relatedProducts = _context.TbProducts
                                  .Where(p => p.CategoryProductId == product.CategoryProductId && p.IsActive)
                                  .Take(4)
                                  .ToList();
            ViewBag.RelatedProducts = relatedProducts;

            return View(product);
        }

        private FilterOptionsProductView GetFilterOptions()
        {
            var filterOptions = new FilterOptionsProductView
            {
                Colors = [.. _context.TbProducts.Select(p => p.Color).Distinct()],
                CpuCompanies = [.. _context.TbProducts.Select(p => p.CpuCompany).Distinct()],
                RamSize = [.. _context.TbProducts.Select(p => p.RamSize).Distinct()],
                ScreenSizes = [.. _context.TbProducts.Select(p => p.ScreenSize).Distinct()],
                DriveMemory = [.. _context.TbProducts.Select(p => p.DriveMemory).Distinct()]
            };

            return filterOptions;
        }
    }
}
