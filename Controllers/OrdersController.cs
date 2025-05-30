using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;
        public OrdersController(AppDbContext db, IDiscountService discountService, OrderService orderService)
        {
            _db = db;
            _discountService = discountService;
            _orderService = orderService;
        }

        /// <summary>
        /// Creates a new order for the specified customer and applies discount based on customer segment.
        /// </summary>
        /// <param name="customerId">ID of the customer placing the order</param>
        /// <param name="baseAmount">Original amount before discount</param>
        /// <returns>The created order details including applied discount</returns>
        /// <response code="200">Order created successfully</response>
        /// <response code="404">Customer not found</response>
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
                CreatedAt = DateTime.UtcNow
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            var response = new OrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt
            };

            return Ok(response);
        }

        /// <summary>
        /// Updates the status of an existing order.
        /// </summary>
        /// <param name="id">ID of the order to update</param>
        /// <param name="newStatus">New status to set for the order</param>
        /// <returns>The updated order if successful</returns>
        /// <response code="200">Order status updated successfully</response>
        /// <response code="400">Invalid status transition</response>
        /// <response code="404">Order not found</response>
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

        /// <summary>
        /// Retrieves analytics for delivered orders including average order value and average fulfillment time in hours.
        /// Added caching to improve performance.
        /// </summary>
        /// <returns>Analytics data for delivered orders</returns>
        [HttpGet("analytics")]
        public IActionResult GetAnalytics()
        {
            const string cacheKey = "orders_analytics";
            if (!_cache.TryGetValue(cacheKey, out var analytics))
            {
                var orders = _db.Orders
                    .AsNoTracking()
                    .Where(o => o.Status == OrderStatus.Delivered)
                    .ToList();

                var avgValue = orders.Any() ? orders.Average(o => o.TotalAmount) : 0;
                var avgHours = orders
                    .Where(o => o.DeliveredAt != null)
                    .Average(o => (o.DeliveredAt - o.CreatedAt)?.TotalHours ?? 0);

                analytics = new { avgValue, avgFulfillmentHours = avgHours };

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                _cache.Set(cacheKey, analytics, cacheOptions);
            }

            return Ok(analytics);
        }

    }
}
