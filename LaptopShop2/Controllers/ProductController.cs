using LaptopShop2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopShop2.Functions;
using LaptopShop2.ViewModels;
using X.PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LaptopShop2.Controllers
{
    public class ProductController : Controller
    {
        private readonly LaptopShopContext _context;
        public ProductController(LaptopShopContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page, int? categoryId, string color, string cpuCompany, string ramSize, string screenSize, string driveMemory)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            var products = _context.TbProducts.Include(p => p.Brand).Include(p => p.CategoryProduct).AsQueryable();

            if (!string.IsNullOrEmpty(color))
            {
                products = products.Where(p => p.Color == color);
            }
            if (!string.IsNullOrEmpty(cpuCompany))
            {
                products = products.Where(p => p.CpuCompany == cpuCompany);
            }
            if (!string.IsNullOrEmpty(ramSize))
            {
                products = products.Where(p => p.RamSize == ramSize);
            }
            if (!string.IsNullOrEmpty(screenSize))
            {
                products = products.Where(p => p.ScreenSize == screenSize);
            }
            if (!string.IsNullOrEmpty(driveMemory))
            {
                products = products.Where(p => p.DriveMemory == driveMemory);
            }

            ViewBag.Brand = _context.TbBrands.ToList();
            ViewBag.Categories = _context.TbCategoryProducts.ToList();
            ViewBag.FilterOptions = GetFilterOptions();
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.FilterOptions = GetFilterOptions();
            ViewBag.SelectedColor = color;
            ViewBag.SelectedCpuCompany = cpuCompany;
            ViewBag.SelectedRamSize = ramSize;
            ViewBag.SelectedScreenSize = screenSize;
            ViewBag.SelectedDriveMemory = driveMemory;

            var totalRecords = products.Count();
            var pageProduct = products.OrderByDescending(p => p.ProductId)
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();

            var pagedList = new StaticPagedList<TbProduct>(pageProduct, pageNumber, pageSize, totalRecords);

            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("ProductListPartial", pagedList);
            }
            else
            {
                return View(pagedList);
            }
        }

        //public IActionResult Index(int? page,int? categoryId)
        //{
        //    int pageSize = 4;
        //    int pageNumber = (page ?? 1);

        //    var products = _context.TbProducts.Include(p => p.Brand).Include(p => p.CategoryProduct).AsQueryable();


        //    var brand = _context.TbBrands.ToList();
        //    ViewBag.Brand = brand;

        //    if (categoryId.HasValue)
        //    {
        //        products = (IQueryable<TbProduct>)products.Where(p => p.CategoryProductId == categoryId).ToList();
        //    }
        //    ViewBag.Categories = _context.TbCategoryProducts.ToList();
        //    ViewBag.SelectedCategoryId = categoryId;
        //    // Lấy các tùy chọn lọc và gán chúng vào ViewBag hoặc ViewData
        //    ViewBag.FilterOptions = GetFilterOptions();
        //    var totalRecords = products.Count();
        //    var pageProduct = products.OrderByDescending(p => p.ProductId)
        //                        .Skip((pageNumber - 1) * pageSize)
        //                        .Take(pageSize)
        //                        .ToList();

        //    var pagedList = new StaticPagedList<TbProduct>(pageProduct, pageNumber, pageSize, totalRecords);
        //    return View(pagedList);
        //}


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
                Colors = _context.TbProducts.Select(p => p.Color).Distinct().ToList(),
                CpuCompanies = _context.TbProducts.Select(p => p.CpuCompany).Distinct().ToList(),
                RamSize = _context.TbProducts.Select(p => p.RamSize).Distinct().ToList(),
                ScreenSizes = _context.TbProducts.Select(p => p.ScreenSize).Distinct().ToList(),
                DriveMemory = _context.TbProducts.Select(p => p.DriveMemory).Distinct().ToList()
            };

            return filterOptions;
        }
    }
}
