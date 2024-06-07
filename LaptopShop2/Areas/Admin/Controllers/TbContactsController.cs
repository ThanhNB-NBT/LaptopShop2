using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopShop2.Models;
using X.PagedList;
using LaptopShop2.Functions;
using Microsoft.AspNetCore.Authorization;

namespace LaptopShop2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class TbContactsController : Controller
    {
        private readonly LaptopShopContext _context;

        public TbContactsController(LaptopShopContext context)
        {
            _context = context;
        }

        // GET: Admin/TbContacts
        public async Task<IActionResult> Index(int? page, string searchString)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            IQueryable<TbContact> contacts = _context.TbContacts;

            if (!string.IsNullOrEmpty(searchString))
            {
                contacts = contacts.Where(c => c.Email.Contains(searchString));
            }

            var pageContacts = await contacts.ToPagedListAsync(pageNumber, pageSize);

            ViewBag.SearchString = searchString;

            return View(pageContacts);
        }

        // GET: Admin/TbContacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbContact = await _context.TbContacts
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (tbContact == null)
            {
                return NotFound();
            }

            tbContact.IsRead = true;
            _context.SaveChanges();

            return View(tbContact);
        }

        // GET: Admin/TbContacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbContact = await _context.TbContacts
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (tbContact == null)
            {
                return NotFound();
            }

            return View(tbContact);
        }

        // POST: Admin/TbContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var tbContact = await _context.TbContacts.FindAsync(id);
                if (tbContact != null)
                {
                    _context.TbContacts.Remove(tbContact);
                }
                TempData["SuccessMessage"] = "Xóa thành công!";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                return View();
            }
        }

        private bool TbContactExists(int id)
        {
            return _context.TbContacts.Any(e => e.ContactId == id);
        }
    }
}
