using LaptopShop2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LaptopShop2.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly LaptopShopContext _context;
        public CustomerController(LaptopShopContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.TbCustomers.FindAsync(int.Parse(userId));

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        

        // POST: Admin/TbAccounts/UpdateProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(TbCustomer model)
        {
            if (!ModelState.IsValid)
            {
                return View("Profile", model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.TbCustomers.FindAsync(int.Parse(userId));

            if (user == null)
            {
                return NotFound();
            }

            user.FullName = model.FullName;
            user.Address = model.Address;
            user.Phone = model.Phone;
            user.Email = model.Email;

            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
