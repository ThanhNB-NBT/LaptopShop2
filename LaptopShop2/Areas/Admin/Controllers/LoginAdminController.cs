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

namespace LaptopShop2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginAdminController : Controller
    {
        private readonly LaptopShopContext _context;
        public LoginAdminController(LaptopShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Register(TbAccount account)
        {
            if (!ModelState.IsValid)
            {
                return View(account);
            }

            // Kiểm tra xem tên đăng nhập đã tồn tại chưa
            if (await _context.TbAccounts.AnyAsync(u => u.Username == account.Username))
            {
                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại.");
                return View(account);
            }

            // Mã hóa mật khẩu
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);


            account.IsActive = true;
            account.RoleId = 1;

            _context.TbAccounts.Add(account);
            await _context.SaveChangesAsync();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Vui lòng nhập tên đăng nhập và mật khẩu.");
                return View();
            }

            var user = await _context.TbAccounts
                                   .Include(u => u.Role)
                                   .FirstOrDefaultAsync(u => u.Username == username && u.IsActive == true);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "Admin"),
                new Claim("FullName", user.FullName ?? ""),
                new Claim("Avatar", user.Avatar ?? "")
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Lưu cookie đăng nhập ngay cả khi đóng trình duyệt
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1) // Cookie hết hạn sau 7 ngày
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home"); // Điều hướng đến trang chủ sau khi đăng nhập thành công
            }
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "LoginAdmin", new { area = "Admin" });
        }
    }
}
