using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaptopShop2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/file-manager")]
    [Authorize]
    public class FileManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
