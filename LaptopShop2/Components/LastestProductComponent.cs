using LaptopShop2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LaptopShop2.Components
{
    [ViewComponent(Name = "LastestProduct")]
    public class LastestProductComponent(LaptopShopContext context) : ViewComponent
    {
        private readonly LaptopShopContext _context = context;

        public IViewComponentResult Invoke()
        {
            var lastestProduct = _context.TbProducts
                                   .Where(p => p.IsActive && p.IsNew)
                                   .OrderByDescending(p => p.ProductId)
                                   .Take(6)
                                   .ToList();

            return View("Default", lastestProduct);
        }
    }
}
