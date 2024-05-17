using LaptopShop2.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaptopShop2.Components
{
    [ViewComponent(Name = "BrandView")]
    public class BrandViewComponent : ViewComponent
    {
        private readonly LaptopShopContext _context;
        public BrandViewComponent(LaptopShopContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listBrand = _context.TbBrands.ToList();

            return await Task.FromResult((IViewComponentResult)View("Default", listBrand));
        }
    }
}
