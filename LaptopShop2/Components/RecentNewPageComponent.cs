using LaptopShop2.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaptopShop2.Components
{
    [ViewComponent(Name = "RecentNewPage")]
    public class RecentNewPageComponent : ViewComponent
    {
        private readonly LaptopShopContext _context;
        public RecentNewPageComponent(LaptopShopContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listofNews = (from p in _context.TbNews
                              where (p.IsActive == true)
                              orderby p.NewId descending
                              select p).Take(3).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listofNews));
        }
    }
}
