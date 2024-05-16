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
using System.ComponentModel.DataAnnotations;

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
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,Description,Detail,Image,CreatedDate,Price,Discount,Quantity,IsNew,IsBestSell,IsActive,Code,Origin,Size,Weight,Color,Material,CpuCompany,CpuType,CpuSpeed,CpuMaxSpeed,CpuCore,CpuProcessor,RamSize,RamType,RamSpeed,RamSupportMax,ScreenSize,ScreenPixel,ScreenPanel,CardBrand,CardModel,DriveType,DriveMemory,ConnectPort,Wifi,Bluetooth,Webcam,PinType,PinCapacity,Os,Version,CategoryProductId,BrandId")] TbProduct tbProduct)
        {
            if (id != tbProduct.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tbProduct.CreatedDate = DateTime.Now;
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

        //Upload hình ảnh
        public class UploadOneFile
        {
            [Required(ErrorMessage = "Phải chọn file upload")]
            [DataType(DataType.Upload)]
            [FileExtensions(Extensions = "png,jpg,jpeg,gif,jfif,webp,svg")]
            //[Display(Name = " Chọn file Upload")]
            public IFormFile? FileUpload { get; set; }
        }

        [HttpGet]
        public IActionResult UploadImage(int id)
        {
            var product = _context.TbProducts.Where(p => p.ProductId == id)
                                            .Include(i => i.TbImages)
                                            .FirstOrDefault();

            if (product == null)
            {
                return NotFound("Không có sản phẩm");
            }
            ViewData["product"] = product;
            return View(new UploadOneFile());
        }

        [HttpPost, ActionName("UploadImage")]
        public async Task<IActionResult> UploadImageAsync(int id, [Bind("FileUpload")] UploadOneFile f)
        {
            var product = _context.TbProducts.Where(p => p.ProductId == id)
                                            .Include(i => i.TbImages)
                                            .FirstOrDefault();
            if (product == null)
            {
                return NotFound("Không có sản phẩm");
            }
            ViewData["product"] = product;

            if (f != null)
            {
                var file1 = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                            + Path.GetExtension(f.FileUpload.FileName);
                var file = Path.Combine("wwwroot", "files", file1);

                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await f.FileUpload.CopyToAsync(fileStream);
                }
                _context.Add(new TbImage()
                {
                    ProductId = product.ProductId,
                    ImagePath = file1
                });

                await _context.SaveChangesAsync();

            }

            return View(f);
        }

        [HttpPost]
        public IActionResult ListImages(int id)
        {
            var product = _context.TbProducts.Where(p => p.ProductId == id)
                                            .Include(i => i.TbImages)
                                            .FirstOrDefault();
            if (product == null)
            {
                return Json(
                    new
                    {
                        success = 0,
                        message = "Product not found",
                    }
                );
            }
            var listImages = product.TbImages.Select(Image => new
            {
                id = Image.ImageId,
                path = "/files/" + Image.ImagePath
            });
            return Json(
                new
                {
                    success = 1,
                    images = listImages,
                }
            );
        }

        [HttpPost]
        public IActionResult DeleteImage(int id)
        {
            var image = _context.TbImages.Where(i => i.ImageId == id).FirstOrDefault();
            if (image != null)
            {
                _context.Remove(image);
                _context.SaveChanges();

                var filename = "wwwroot/files/" + image.ImagePath;
                System.IO.File.Delete(filename);
            }
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> UploadImageApi(int id, [Bind("FileUpload")] UploadOneFile f)
        {
            var product = _context.TbProducts.Where(p => p.ProductId == id)
                                            .Include(i => i.TbImages)
                                            .FirstOrDefault();
            if (product == null)
            {
                return NotFound("Không có sản phẩm");
            }

            if (f != null)
            {
                var file1 = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                            + Path.GetExtension(f.FileUpload.FileName);
                var file = Path.Combine("wwwroot", "files", file1);

                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await f.FileUpload.CopyToAsync(fileStream);
                }
                _context.Add(new TbImage()
                {
                    ProductId = product.ProductId,
                    ImagePath = file1
                });

                await _context.SaveChangesAsync();

            }
            return Ok();
        }
    }
}
