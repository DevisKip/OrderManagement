using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Data;
using OrderManagement.Models;
using OrderManagement.Services;

namespace OrderManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IDiscountService _discountService;
        private readonly OrderService _orderService;

        public OrdersController(AppDbContext db, IDiscountService discountService, OrderService orderService)
        {
            _db = db;
            _discountService = discountService;
            _orderService = orderService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromQuery] int customerId, [FromQuery] decimal baseAmount)
        {
            var customer = await _db.Customers.Include(c => c.Orders).FirstOrDefaultAsync(c => c.Id == customerId);
            if (customer is null) return NotFound("Customer not found");

            var finalAmount = _discountService.ApplyDiscount(customer, baseAmount);

            var order = new Order
            {
                CustomerId = customerId,
                TotalAmount = finalAmount,
                CreatedAt=DateTime.UtcNow
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            var response = new OrderResponse
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt
            };

            return Ok(response);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] OrderStatus newStatus)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null) return NotFound();

            if (_orderService.UpdateStatus(order, newStatus))
            {
                await _db.SaveChangesAsync();
                return Ok(order);
            }

            return BadRequest("Invalid status transition.");
        }

        [HttpGet("analytics")]
        public IActionResult GetAnalytics()
        {
            var orders = _db.Orders
                .AsNoTracking()
                .Where(o => o.Status == OrderStatus.Delivered)
                .ToList();

            var avgValue = orders.Any() ? orders.Average(o => o.TotalAmount) : 0;
            var avgHours = orders
                .Where(o => o.DeliveredAt != null)
                .Average(o => (o.DeliveredAt - o.CreatedAt)?.TotalHours ?? 0);

            return Ok(new { avgValue, avgFulfillmentHours = avgHours });
        }
    }
}
