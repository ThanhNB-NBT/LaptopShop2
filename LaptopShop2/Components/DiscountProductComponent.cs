using LaptopShop2.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaptopShop2.Components
{
    [ViewComponent(Name = "DiscountProducts")]
    public class DiscountProductComponent(LaptopShopContext context) : ViewComponent
    {
        private readonly LaptopShopContext _context = context;

        public IViewComponentResult Invoke()
        {
            var discountProduct = _context.TbProducts
                                   .Where(p => p.IsActive)
                                   .OrderByDescending(p => p.Discount)
                                   .Take(6)
                                   .ToList();

            return View("Default", discountProduct);
        }
    }
}
