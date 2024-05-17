using LaptopShop2.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaptopShop2.Components
{
    [ViewComponent(Name = "HeroSectionView")]
    public class HeroSectionViewComponent : ViewComponent
    {
        private readonly LaptopShopContext _context;

        public HeroSectionViewComponent(LaptopShopContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listofBrand = (from b in _context.TbBrands
                               select b).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listofBrand));
        }
    }
}
