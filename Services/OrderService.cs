using OrderManagement.Models;

namespace OrderManagement.Services
{
    public class OrderService
    {
        private static readonly Dictionary<OrderStatus, OrderStatus[]> ValidTransitions = new()
        {
            [OrderStatus.Pending] = new[] { OrderStatus.Processing, OrderStatus.Cancelled },
            [OrderStatus.Processing] = new[] { OrderStatus.Shipped, OrderStatus.Cancelled },
            [OrderStatus.Shipped] = new[] { OrderStatus.Delivered }
        };

        public bool UpdateStatus(Order order, OrderStatus newStatus)
        {
            if (ValidTransitions.TryGetValue(order.Status, out var allowed) && allowed.Contains(newStatus))
            {
                order.Status = newStatus;
                if (newStatus == OrderStatus.Shipped) order.ShippedAt = DateTime.UtcNow;
                if (newStatus == OrderStatus.Delivered) order.DeliveredAt = DateTime.UtcNow;
                return true;
            }

            return false;
        }
    }
}
