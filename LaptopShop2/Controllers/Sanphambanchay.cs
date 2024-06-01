using LaptopShop2.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaptopShop2.Controllers
{
    public class Sanphambanchay : Controller
    {
        private readonly LaptopShopContext _context;
        public Sanphambanchay(LaptopShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var spbc = (from m in _context.TbProducts
                        where m.IsActive == true
                        orderby m.CreatedDate descending
                        select m).Take(3).ToList();

            ViewBag.spBanChay = spbc;
            return View();
        }
    }
}
