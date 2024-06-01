using LaptopShop2.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LaptopShop2.Controllers
{
    public class ContactController : Controller
    {
        private readonly LaptopShopContext _context;

        public ContactController(LaptopShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendContactInfo(TbContact model)
        {
            if (ModelState.IsValid)
            {
                // Lưu thông tin liên hệ vào database
                model.CreateDate = DateTime.Now;
                model.IsRead = false;
                _context.TbContacts.Add(model);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Thông tin của bạn đã được gửi đi" });
            }

            return Json(new { success = false, message = "Có lỗi xảy ra. Vui lòng thử lại sau." });
        }
    }
}
