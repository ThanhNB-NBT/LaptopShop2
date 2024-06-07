using LaptopShop2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LaptopShop2.Controllers
{
    [Authorize(AuthenticationSchemes = "UserCookie")]
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


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Lấy danh sách đơn hàng của khách hàng cụ thể
            var orders = await _context.TbOrders
                                        .Include(t => t.Customer)
                                        .Include(t => t.OrderStatus)
                                        .Include(t => t.TbOrderDetails)
                                        .ThenInclude(od => od.Product)
                                        .Where(m => m.CustomerId == id)
                                        .ToListAsync();
            if (orders == null || orders.Count == 0)
            {
                return NotFound();
            }

            var orderStatuses = await _context.TbOrderStatuses.ToListAsync();
            ViewBag.OrderStatuses = new SelectList(orderStatuses, "OrderStatusId", "Name");

            return View(orders);
        }
    }
}
