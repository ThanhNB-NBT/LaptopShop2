using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopShop2.Models;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;

namespace LaptopShop2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TbBrandsController : Controller
    {
        private readonly LaptopShopContext _context;

        public TbBrandsController(LaptopShopContext context)
        {
            _context = context;
        }

        // GET: Admin/TbBrands
        public async Task<IActionResult> Index(int? page, string searchString)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            IQueryable<TbBrand> brands = _context.TbBrands;
            if (!string.IsNullOrEmpty(searchString))
            {
                brands = brands.Where(b => b.Name.Contains(searchString));
            }
            var pageBrands = await brands.ToPagedListAsync(pageNumber, pageSize);
            ViewBag.SearchString = searchString;
            return View(pageBrands);
        }

        // GET: Admin/TbBrands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbBrand = await _context.TbBrands
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (tbBrand == null)
            {
                return NotFound();
            }

            return View(tbBrand);
        }

        // GET: Admin/TbBrands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/TbBrands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrandId,Name,Banner,Description")] TbBrand tbBrand)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(tbBrand);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Thêm thương hiệu thành công!";
                    return RedirectToAction(nameof(Index));
                } 
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(tbBrand);
        }

        // GET: Admin/TbBrands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbBrand = await _context.TbBrands.FindAsync(id);
            if (tbBrand == null)
            {
                return NotFound();
            }
            return View(tbBrand);
        }

        // POST: Admin/TbBrands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BrandId,Name,Banner,Description")] TbBrand tbBrand)
        {
            if (id != tbBrand.BrandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbBrand);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm thương hiệu thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbBrandExists(tbBrand.BrandId))
                    {
                        TempData["ErrorMessage"] = "Có lỗi xảy ra, hãy thử lại!";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tbBrand);
        }

        // GET: Admin/TbBrands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbBrand = await _context.TbBrands
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (tbBrand == null)
            {
                return NotFound();
            }

            return View(tbBrand);
        }

        // POST: Admin/TbBrands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbBrand = await _context.TbBrands.FindAsync(id);
            if (tbBrand != null)
            {
                _context.TbBrands.Remove(tbBrand);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa thương hiệu thành công!";
            return RedirectToAction(nameof(Index));
        }

        private bool TbBrandExists(int id)
        {
            return _context.TbBrands.Any(e => e.BrandId == id);
        }
    }
}
