using OrderManagement.Models;

namespace OrderManagement.Services
{
    public class DiscountService : IDiscountService
    {
        public decimal ApplyDiscount(Customer customer, decimal amount)
        {
            return customer.Segment switch
            {
                CustomerSegment.Premium when customer.Orders.Count > 5 => amount * 0.95m,
                CustomerSegment.VIP => amount * 0.90m,
                _ => amount
            };
        }
    }
}
