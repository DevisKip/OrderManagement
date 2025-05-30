using OrderManagement.Models;

namespace OrderManagement.Services
{
    public interface IDiscountService
    {
        decimal ApplyDiscount(Customer customer, decimal amount);
    }
}
