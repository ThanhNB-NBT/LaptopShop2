using LaptopShop2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace LaptopShop2.Areas.Admin.Components
{
    [ViewComponent(Name = "AdminMenu")]
    public class AdminMenuComponent(LaptopShopContext context) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var mnList = await context.TbMenuAdmins
                         .Where(mn => mn.IsActive)
                         .ToListAsync();
            return View("Default", mnList);
        }
    }
}
