﻿using Microsoft.AspNetCore.Mvc;

namespace LaptopShop2.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
