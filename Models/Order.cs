using System;

namespace OrderManagement.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public Customer Customer { get; set; }
    }

}
