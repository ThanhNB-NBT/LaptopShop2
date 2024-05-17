using Microsoft.AspNetCore.Mvc;

namespace LaptopShop2.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
