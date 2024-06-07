using LaptopShop2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace LaptopShop2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class TbMenusController : Controller
    {
        private readonly LaptopShopContext _context;

        public TbMenusController(LaptopShopContext context)
        {
            _context = context;
        }

        // GET: Admin/TbMenus
        public async Task<IActionResult> Index(int? page, string searchString)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            IQueryable<TbMenu> menus = _context.TbMenus;
            if (!string.IsNullOrEmpty(searchString))
            {
                menus = menus.Where(m => m.MenuName.Contains(searchString));
            }
            var pageMenus = await menus.ToPagedListAsync(pageNumber, pageSize);
            ViewBag.SearchString = searchString;
            return View(pageMenus);
        }

        // GET: Admin/TbMenus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbMenu = await _context.TbMenus
                .FirstOrDefaultAsync(m => m.MenuId == id);
            if (tbMenu == null)
            {
                return NotFound();
            }

            return View(tbMenu);
        }

        // GET: Admin/TbMenus/Create
        public IActionResult Create()
        {
            ViewBag.ParentMenus = new SelectList(_context.TbMenus, "MenuId", "MenuName");
            return View();
        }

        // POST: Admin/TbMenus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuId,MenuName,ControllerName,ActionName,MenuLevel,MenuOrder,ParentId,Position,IsActive")] TbMenu tbMenu)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(tbMenu);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Menu đã được thêm!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    TempData["ErrorMessage"] = "Có lỗi xảy ra. Vui lòng thử lại!";
                }
            }
            ViewBag.ParentMenus = new SelectList(_context.TbMenus, "MenuId", "MenuName");
            return View(tbMenu);
        }

        // GET: Admin/TbMenus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbMenu = await _context.TbMenus.FindAsync(id);
            if (tbMenu == null)
            {
                return NotFound();
            }
            ViewBag.ParentMenus = new SelectList(_context.TbMenus, "MenuId", "MenuName");
            return View(tbMenu);
        }

        // POST: Admin/TbMenus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MenuId,MenuName,ControllerName,ActionName,MenuLevel,MenuOrder,ParentId,Position,IsActive")] TbMenu tbMenu)
        {
            if (id != tbMenu.MenuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbMenu);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Menu đã được chỉnh sửa!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbMenuExists(tbMenu.MenuId))
                    {
                        TempData["ErrorMessage"] = "Có lỗi xảy ra. Vui lòng thử lại!";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tbMenu);
        }

        // GET: Admin/TbMenus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbMenu = await _context.TbMenus
                .FirstOrDefaultAsync(m => m.MenuId == id);
            if (tbMenu == null)
            {
                return NotFound();
            }

            return View(tbMenu);
        }

        // POST: Admin/TbMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var tbMenu = await _context.TbMenus.FindAsync(id);
                if (tbMenu != null)
                {
                    _context.TbMenus.Remove(tbMenu);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Menu đã được xóa!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Menu không tồn tại";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                TempData["ErrorMessage"] = "Có lỗi xảy ra. Vui lòng thử lại!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TbMenuExists(int id)
        {
            return _context.TbMenus.Any(e => e.MenuId == id);
        }
    }
}