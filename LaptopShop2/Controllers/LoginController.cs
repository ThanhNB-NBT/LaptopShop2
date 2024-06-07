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

namespace LaptopShop2.Controllers
{
    public class LoginController : Controller
    {
        private readonly LaptopShopContext _context;
        public LoginController(LaptopShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Register(TbCustomer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            // Kiểm tra xem tên đăng nhập đã tồn tại chưa
            if (await _context.TbCustomers.AnyAsync(u => u.Username == customer.Username))
            {
                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại.");
                return View(customer);
            }

            // Mã hóa mật khẩu
            customer.Password = BCrypt.Net.BCrypt.HashPassword(customer.Password);


            customer.IsActive = true;

            _context.TbCustomers.Add(customer);
            await _context.SaveChangesAsync();

            return View("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Vui lòng nhập tên đăng nhập và mật khẩu.");
                return View();
            }

            var user = await _context.TbCustomers
                                   .FirstOrDefaultAsync(u => u.Username == username && u.IsActive == true);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.CustomerId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("FullName", user.FullName ?? ""),
                new Claim("CustomerId", user.CustomerId.ToString())
            };

                var claimsIdentity = new ClaimsIdentity(claims, "UserCookie");
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Lưu cookie đăng nhập ngay cả khi đóng trình duyệt
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1) // Cookie hết hạn sau 1 ngày
                };

                await HttpContext.SignInAsync(
                    "UserCookie",
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("UserCookie");
            return RedirectToAction("Index", "Home");
        }
    }
}
