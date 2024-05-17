using LaptopShop2.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaptopShop2.Components
{
    [ViewComponent(Name = "DiscountProducts")]
    public class DiscountProductComponent : ViewComponent
    {
        private readonly LaptopShopContext _context;
        public DiscountProductComponent(LaptopShopContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
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
