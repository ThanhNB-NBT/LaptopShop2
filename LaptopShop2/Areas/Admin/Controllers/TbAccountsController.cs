using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopShop2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

namespace LaptopShop2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class TbAccountsController : Controller
    {
        private readonly LaptopShopContext _context;

        public TbAccountsController(LaptopShopContext context)
        {
            _context = context;
        }

        // GET: Admin/TbAccounts
        public async Task<IActionResult> Index()
        {
            var laptopShopContext = _context.TbAccounts.Include(t => t.Role);
            return View(await laptopShopContext.ToListAsync());
        }

        

        // GET: Admin/TbAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbAccount = await _context.TbAccounts
                .Include(t => t.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (tbAccount == null)
            {
                return NotFound();
            }

            return View(tbAccount);
        }

        // GET: Admin/TbAccounts/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.TbRoles, "RoleId", "RoleId");
            return View();
        }

        // POST: Admin/TbAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Username,Password,Avatar,Phone,Email,Address,RoleId,IsActive")] TbAccount tbAccount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.TbRoles, "RoleId", "RoleId", tbAccount.RoleId);
            return View(tbAccount);
        }

        // GET: Admin/TbAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbAccount = await _context.TbAccounts.FindAsync(id);
            if (tbAccount == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.TbRoles, "RoleId", "RoleId", tbAccount.RoleId);
            return View(tbAccount);
        }

        // POST: Admin/TbAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,Username,Password,Avatar,Phone,Email,Address,RoleId,IsActive")] TbAccount tbAccount)
        {
            if (id != tbAccount.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbAccountExists(tbAccount.AccountId))
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
            ViewData["RoleId"] = new SelectList(_context.TbRoles, "RoleId", "RoleId", tbAccount.RoleId);
            return View(tbAccount);
        }

        // GET: Admin/TbAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbAccount = await _context.TbAccounts
                .Include(t => t.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (tbAccount == null)
            {
                return NotFound();
            }

            return View(tbAccount);
        }

        // POST: Admin/TbAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbAccount = await _context.TbAccounts.FindAsync(id);
            if (tbAccount != null)
            {
                _context.TbAccounts.Remove(tbAccount);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbAccountExists(int id)
        {
            return _context.TbAccounts.Any(e => e.AccountId == id);
        }

        // GET: Admin/TbAccounts/Profile
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.TbAccounts.FindAsync(int.Parse(userId));

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/TbAccounts/UpdateProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(TbAccount model, IFormFile avatar)
        {
            if (!ModelState.IsValid)
            {
                return View("Profile", model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.TbAccounts.FindAsync(int.Parse(userId));

            if (user == null)
            {
                return NotFound();
            }

            if (avatar != null && avatar.Length > 0)
            {
                var fileName = Path.GetFileName(avatar.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await avatar.CopyToAsync(fileStream);
                }
                model.Avatar = "/files/" + fileName;
            }

            user.FullName = model.FullName;
            user.Address = model.Address;
            user.Phone = model.Phone;
            user.Email = model.Email;

            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Profile");
        }
    }
}
