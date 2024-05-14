using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopShop2.Models;
using X.PagedList;

namespace LaptopShop2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TbNewsController : Controller
    {
        private readonly LaptopShopContext _context;

        public TbNewsController(LaptopShopContext context)
        {
            _context = context;
        }

        // GET: Admin/TbNews
        public async Task<IActionResult> Index(int? page, string searchString)
        {

            
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            var news = _context.TbNews.Include(t => t.CategoryNew).OrderByDescending(b => b.CreatedDate);

            if (!string.IsNullOrEmpty(searchString))
            {
                news = (IOrderedQueryable<TbNews>)news.Where(b => b.Title.Contains(searchString));
            }

            var pageList = await news.ToPagedListAsync(pageNumber, pageSize);
            ViewBag.SearchString = searchString;
            return View(pageList);
        }

        // GET: Admin/TbNews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbNews = await _context.TbNews
                .Include(t => t.CategoryNew)
                .FirstOrDefaultAsync(m => m.NewId == id);
            if (tbNews == null)
            {
                return NotFound();
            }

            return View(tbNews);
        }

        // GET: Admin/TbNews/Create
        public IActionResult Create()
        {
            ViewData["CategoryNewId"] = new SelectList(_context.TbCategoryNews, "CategoryNewId", "Name");
            var news = new TbNews
            {
                CreatedDate = DateTime.Now
            };
            return View(news);
        }

        // POST: Admin/TbNews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewId,Title,Description,Detail,CreatedDate,CreatedBy,Image,Tags,CategoryNewId,IsActive")] TbNews tbNews, string CategoryInput)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(CategoryInput))
                    {
                        var CategoryName = _context.TbCategoryNews.FirstOrDefault(c => c.Name == CategoryInput);
                        if (CategoryName == null)
                        {
                            var newCategoryName = new TbCategoryNew { Name = CategoryInput };
                            _context.TbCategoryNews.Add(newCategoryName);
                            await _context.SaveChangesAsync();

                            tbNews.CategoryNewId = newCategoryName.CategoryNewId;
                        }
                        else
                        {
                            tbNews.CategoryNewId = CategoryName.CategoryNewId;
                        }
                    }
                    _context.Add(tbNews);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm tin tức thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi:" + ex.Message);
                    TempData["ErrorMessage"] = "Có lỗi xảy ra. Vui lòng thử lại!";
                }
            }
            ViewData["CategoryNewId"] = new SelectList(_context.TbCategoryNews, "CategoryNewId", "Name", tbNews.CategoryNewId);
            return View(tbNews);
        }

        // GET: Admin/TbNews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbNews = await _context.TbNews.FindAsync(id);
            if (tbNews == null)
            {
                return NotFound();
            }
            ViewData["CategoryNewId"] = new SelectList(_context.TbCategoryNews, "CategoryNewId", "Name", tbNews.CategoryNewId);
            return View(tbNews);
        }

        // POST: Admin/TbNews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NewId,Title,Description,Detail,CreatedDate,CreatedBy,Image,Tags,CategoryNewId,IsActive")] TbNews tbNews)
        {
            if (id != tbNews.NewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbNews);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Tin tức đã được chỉnh sửa!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbNewsExists(tbNews.NewId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryNewId"] = new SelectList(_context.TbCategoryNews, "CategoryNewId", "Name", tbNews.CategoryNewId);
            return View(tbNews);
        }

        // GET: Admin/TbNews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbNews = await _context.TbNews
                .Include(t => t.CategoryNew)
                .FirstOrDefaultAsync(m => m.NewId == id);
            if (tbNews == null)
            {
                return NotFound();
            }

            return View(tbNews);
        }

        // POST: Admin/TbNews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var tbBlog = await _context.TbNews.FindAsync(id);
                if (tbBlog != null)
                {
                    _context.TbNews.Remove(tbBlog);
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa Tin tức!";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                TempData["ErrorMessage"] = "Có lỗi xảy ra. Vui lòng thử lại!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TbNewsExists(int id)
        {
            return _context.TbNews.Any(e => e.NewId == id);
        }
    }
}
