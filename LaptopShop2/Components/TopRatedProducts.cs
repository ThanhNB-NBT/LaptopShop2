using LaptopShop2.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaptopShop2.Components
{
    [ViewComponent(Name = "TopRatedProducts")]
    public class TopRatedProducts : ViewComponent
    {
        private readonly LaptopShopContext _context;
        public TopRatedProducts(LaptopShopContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
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
