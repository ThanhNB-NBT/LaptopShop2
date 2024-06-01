using LaptopShop2.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaptopShop2.Components
{
    [ViewComponent(Name = "TopRatedProducts")]
    public class TopRatedProducts(LaptopShopContext context) : ViewComponent
    {
        private readonly LaptopShopContext _context = context;

        public IViewComponentResult Invoke()
        {
            var topRatedProduct = _context.TbProducts
                                   .Where(p => p.IsActive && p.IsBestSell)
                                   .OrderByDescending(p => p.ProductId)
                                   .Take(6)
                                   .ToList();

            return View("Default", topRatedProduct);
        }
    }
}
