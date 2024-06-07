using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopShop2.Models;
using X.PagedList;
using LaptopShop2.Functions;
using Microsoft.AspNetCore.Authorization;

namespace LaptopShop2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class TbOrdersController : Controller
    {
        private readonly LaptopShopContext _context;
        private readonly ILogger<TbOrdersController> _logger;

        public TbOrdersController(LaptopShopContext context, ILogger<TbOrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Admin/TbOrders
        public async Task<IActionResult> Index(int? page, string searchString)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            IQueryable<TbOrder> order = _context.TbOrders
                                                .Include(t => t.Customer)
                                                .Include(t => t.OrderStatus)
                                                .Include(t => t.TbOrderDetails)
                                                .ThenInclude(od => od.Product);

            if (!string.IsNullOrEmpty(searchString))
            {
                order = order.Where(o => o.CustomerName.Contains(searchString));
            }

            var pageOrders = await order.ToPagedListAsync(pageNumber, pageSize);

            var orderStatuses = await _context.TbOrderStatuses.ToListAsync();
            ViewBag.OrderStatuses = new SelectList(orderStatuses, "OrderStatusId", "Name");

            return View(pageOrders);
        }

        // GET: Admin/TbOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbOrder = await _context.TbOrders
                                        .Include(t => t.Customer)
                                        .Include(t => t.OrderStatus)
                                        .Include(t => t.TbOrderDetails)
                                        .ThenInclude(od => od.Product)
                                        .FirstOrDefaultAsync(m => m.OrderId == id);
            if (tbOrder == null)
            {
                return NotFound();
            }

            var orderStatuses = await _context.TbOrderStatuses.ToListAsync();
            ViewBag.OrderStatuses = new SelectList(orderStatuses, "OrderStatusId", "Name");

            return View(tbOrder);
        }
        // GET: Admin/TbOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbOrder = await _context.TbOrders
                .Include(t => t.Customer)
                .Include(t => t.OrderStatus)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (tbOrder == null)
            {
                return NotFound();
            }

            return View(tbOrder);
        }

        // POST: Admin/TbOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbOrder = await _context.TbOrders.FindAsync(id);
            if (tbOrder != null)
            {
                _context.TbOrders.Remove(tbOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbOrderExists(int id)
        {
            return _context.TbOrders.Any(e => e.OrderId == id);
        }


        [HttpPost]
        public IActionResult UpdateOrderStatus(int orderId, int orderStatusId)
        {
            var order = _context.TbOrders.FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                var status = _context.TbOrderStatuses.FirstOrDefault(s => s.OrderStatusId == orderStatusId);
                if (status != null)
                {
                    order.OrderStatusId = orderStatusId;
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Trạng thái đơn hàng đã được cập nhật thành công.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Trạng thái đơn hàng không tồn tại.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Đơn hàng không tồn tại.";
            }
            return RedirectToAction("Index");
        }

    }
}
