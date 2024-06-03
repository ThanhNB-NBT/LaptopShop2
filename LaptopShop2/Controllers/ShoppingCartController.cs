using LaptopShop2.Functions;
using LaptopShop2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace LaptopShop2.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly LaptopShopContext _context;
        private readonly ISession _session;

        public ShoppingCartController(LaptopShopContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _session = httpContextAccessor.HttpContext.Session;
        }

        public IActionResult Index()
        {
            var cart = _session.GetJson<List<TbOrderDetail>>("Cart") ?? new List<TbOrderDetail>();
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = _context.TbProducts
                                   .Where(p => p.ProductId == productId)
                                   .FirstOrDefault(); // Lấy sản phẩm từ cơ sở dữ liệu
            if (product == null)
                return Json(new { success = false, message = "Sản phẩm không tồn tại." });

            var cart = _session.GetJson<List<TbOrderDetail>>("Cart") ?? new List<TbOrderDetail>();

            var cartItem = cart.FirstOrDefault(c => c.Product.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new TbOrderDetail { ProductId = product.ProductId, Quantity = quantity, Product = product });
            }

            _session.SetJson("Cart", cart);
            return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng." });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = _session.GetJson<List<TbOrderDetail>>("Cart") ?? new List<TbOrderDetail>();

            var cartItem = cart.FirstOrDefault(c => c.Product.ProductId == productId);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
            }

            _session.SetJson("Cart", cart);
            return Json(new { success = true, message = "Sản phẩm đã được xóa khỏi giỏ hàng." });
        }

        [HttpGet]
        public IActionResult GetCartItemCount()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString("Cart");
            int numberOfProducts = 0;

            if (jsoncart != null)
            {
                var cart = JsonConvert.DeserializeObject<List<TbOrderDetail>>(jsoncart);
                numberOfProducts = cart.Count;
            }

            return Json(numberOfProducts);
        }

        [HttpPost]
        public IActionResult Checkout(TbOrder order, decimal totalAmount)
        {
            // Lấy danh sách sản phẩm từ giỏ hàng
            var cart = _session.GetJson<List<TbOrderDetail>>("Cart") ?? new List<TbOrderDetail>();

            if (cart.Count == 0)
            {
                return Json(new { success = false, message = "Giỏ hàng của bạn đang trống." });
            }
            // Lấy thông tin người dùng đã đăng nhập từ claim
            var customerIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (customerIdClaim == null)
            {
                // Nếu không tìm thấy thông tin người dùng, xử lý tùy thuộc vào yêu cầu của ứng dụng của bạn,
                // có thể quay lại trang đăng nhập hoặc yêu cầu đăng nhập trước khi tiếp tục.
                return Json(new { success = false, message = "Vui lòng đăng nhập trước khi thực hiện thanh toán." });
            }

            // Gán customerId cho đơn hàng
            order.CustomerId = Convert.ToInt32(customerIdClaim.Value);
            // Gán tổng tiền cho đơn hàng
            order.TotalAmount = totalAmount;
            order.CreatedDate = DateTime.Now;
            order.OrderStatusId = 1;
            order.IsActive = true;
            // Thêm thông tin đơn hàng vào cơ sở dữ liệu
            _context.TbOrders.Add(order);
            _context.SaveChanges();

            // Lấy OrderId sau khi đã được thêm vào cơ sở dữ liệu
            int orderId = order.OrderId;

            // Gán OrderId và ProductId cho từng chi tiết đơn hàng
            foreach (var item in cart)
            {
                item.OrderId = orderId;
                item.ProductId = item.Product.ProductId; 
                item.Product = null; // Xóa tham chiếu đến sản phẩm để tránh lỗi
                _context.TbOrderDetails.Add(item);
            }

            _context.SaveChanges();

            // Xóa giỏ hàng từ session
            _session.Remove("Cart");

            return RedirectToAction("CustomerInfo", new {orderId = orderId}); 
        }

        public IActionResult CustomerInfo(int orderId)
        {
            // Lấy thông tin đơn hàng từ cơ sở dữ liệu
            var order = _context.TbOrders.Include(o => o.TbOrderDetails)
                                          .ThenInclude(od => od.Product)
                                          .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                // Xử lý khi không tìm thấy đơn hàng
                return NotFound();
            }
            ViewBag.Order = order;
            // Hiển thị form để người dùng điền thông tin khách hàng
            return View(order);
        }
        [HttpPost]
        public IActionResult CustomerInfo(TbOrder order)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }

            // Lấy thông tin đơn hàng từ cơ sở dữ liệu và cập nhật thông tin khách hàng
            var existingOrder = _context.TbOrders.Find(order.OrderId);
            if (existingOrder != null)
            {
                existingOrder.Code = Function.GenerateOrderCode();
                existingOrder.CustomerName = order.CustomerName;
                existingOrder.Phone = order.Phone;
                existingOrder.Address = order.Address;
                existingOrder.Message = order.Message;

                _context.SaveChanges();
            }

            TempData["SuccessMessage"] = "Đặt hàng thành công!";
            return RedirectToAction("Index", "Home");
        }
    }
}
