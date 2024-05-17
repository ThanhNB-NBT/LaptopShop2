using LaptopShop2.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaptopShop2.Components
{
    [ViewComponent(Name = "MenuView")]
    public class MenuViewComponent : ViewComponent
    {
        private readonly LaptopShopContext _context;

        public MenuViewComponent(LaptopShopContext context)
        {
            _context = context;
        }   

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listofMenu = (from m in _context.TbMenus
                              where (m.IsActive) && (m.Position == 1)
                              select m).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listofMenu));
        }
    }
}
