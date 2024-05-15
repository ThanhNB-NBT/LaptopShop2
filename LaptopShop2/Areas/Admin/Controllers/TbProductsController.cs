using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopShop2.Models;
using X.PagedList;
using System.Data;

namespace LaptopShop2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TbProductsController : Controller
    {
        private readonly LaptopShopContext _context;

        public TbProductsController(LaptopShopContext context)
        {
            _context = context;
        }

        // GET: Admin/TbProducts
        public async Task<IActionResult> Index(int? page, string searchString)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            var products = _context.TbProducts.Include(t => t.Brand).Include(t => t.CategoryProduct).OrderByDescending(t => t.CreatedDate);
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString)) as IOrderedQueryable<TbProduct>;
            }
            var pageList = await products.ToPagedListAsync(pageNumber, pageSize);
            return View(pageList);
        }

        // GET: Admin/TbProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbProduct = await _context.TbProducts
                .Include(t => t.Brand)
                .Include(t => t.CategoryProduct)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (tbProduct == null)
            {
                return NotFound();
            }

            return View(tbProduct);
        }

        // GET: Admin/TbProducts/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.TbBrands, "BrandId", "Name");
            ViewData["CategoryProductId"] = new SelectList(_context.TbCategoryProducts, "CategoryProductId", "Name");
            var product = new TbProduct
            {
                CreatedDate = DateTime.Now
            };
            return View(product);
        }

        // POST: Admin/TbProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,Description,Detail,Image,Price,CreatedDate,Discount,Quantity,IsNew,IsBestSell,IsActive,Code,Origin,Size,Weight,Color,Material,CpuCompany,CpuType,CpuSpeed,CpuMaxSpeed,CpuCore,CpuProcessor,RamSize,RamType,RamSpeed,RamSupportMax,ScreenSize,ScreenPixel,ScreenPanel,CardBrand,CardModel,DriveType,DriveMemory,ConnectPort,Wifi,Bluetooth,Webcam,PinType,PinCapacity,Os,Version,CategoryProductId,BrandId")] TbProduct tbProduct, string CategoryInput)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(CategoryInput))
                    {
                        var CategoryName = _context.TbCategoryProducts.FirstOrDefault(c => c.Name == CategoryInput);
                        if (CategoryName == null)
                        {
                            var newCategoryName = new TbCategoryProduct { Name = CategoryInput };
                            _context.TbCategoryProducts.Add(newCategoryName);
                            await _context.SaveChangesAsync();

                            tbProduct.CategoryProductId = newCategoryName.CategoryProductId;
                        }
                        else
                        {
                            tbProduct.CategoryProductId = CategoryName.CategoryProductId;
                        }
                    }
                    _context.Add(tbProduct);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                    return RedirectToAction(nameof(Index));
                } catch(Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    return RedirectToAction(nameof(Index));
                }
               
            }
            ViewData["BrandId"] = new SelectList(_context.TbBrands, "BrandId", "Name", tbProduct.BrandId);
            ViewData["CategoryProductId"] = new SelectList(_context.TbCategoryProducts, "CategoryProductId", "Name", tbProduct.CategoryProductId);
            return View(tbProduct);
        }

        // GET: Admin/TbProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbProduct = await _context.TbProducts.FindAsync(id);
            if (tbProduct == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.TbBrands, "BrandId", "Name", tbProduct.BrandId);
            ViewData["CategoryProductId"] = new SelectList(_context.TbCategoryProducts, "CategoryProductId", "Name", tbProduct.CategoryProductId);
            return View(tbProduct);
        }

        // POST: Admin/TbProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,Description,Detail,Image,CreatedDate,Price,Discount,Quantity,IsNew,IsBestSell,IsActive,Code,Origin,Size,Weight,Color,Material,CpuCompany,CpuType,CpuSpeed,CpuMaxSpeed,CpuCore,CpuProcessor,RamSize,RamType,RamSpeed,RamSupportMax,ScreenSize,ScreenPixel,ScreenPanel,CardBrand,CardModel,DriveType,DriveMemory,ConnectPort,Wifi,Bluetooth,Webcam,PinType,PinCapacity,Os,Version,CategoryProductId,BrandId")] TbProduct tbProduct, string CategoryInput)
        {
            if (id != tbProduct.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(CategoryInput))
                    {
                        var CategoryName = _context.TbCategoryProducts.FirstOrDefault(c => c.Name == CategoryInput);
                        if (CategoryName == null)
                        {
                            var newCategoryName = new TbCategoryProduct { Name = CategoryInput };
                            _context.TbCategoryProducts.Add(newCategoryName);
                            await _context.SaveChangesAsync();

                            tbProduct.CategoryProductId = newCategoryName.CategoryProductId;
                        }
                        else
                        {
                            tbProduct.CategoryProductId = CategoryName.CategoryProductId;
                        }
                    }
                    _context.Update(tbProduct);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Sửa sản phẩm thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbProductExists(tbProduct.ProductId))
                    {
                        TempData["ErrorMessage"] = "Có lỗi xảy ra!";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.TbBrands, "BrandId", "Name", tbProduct.BrandId);
            ViewData["CategoryProductId"] = new SelectList(_context.TbCategoryProducts, "CategoryProductId", "Name", tbProduct.CategoryProductId);
            return View(tbProduct);
        }

        // GET: Admin/TbProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbProduct = await _context.TbProducts
                .Include(t => t.Brand)
                .Include(t => t.CategoryProduct)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (tbProduct == null)
            {
                return NotFound();
            }

            return View(tbProduct);
        }

        // POST: Admin/TbProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var tbProduct = await _context.TbProducts.FindAsync(id);
                if (tbProduct != null)
                {
                    _context.TbProducts.Remove(tbProduct);
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa thành công!";

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TbProductExists(int id)
        {
            return _context.TbProducts.Any(e => e.ProductId == id);
        }
    }
}
