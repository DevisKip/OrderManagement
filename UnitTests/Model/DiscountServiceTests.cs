
using OrderManagement.Models;
using OrderManagement.Services;
using Xunit;

namespace OrderManagement.UnitTests.Model
{
    public class DiscountServiceTests
    {
        private readonly DiscountService _discountService = new();

        [Fact]
        public void VIP_Customer_Gets_20_Percent_Discount()
        {
            var customer = new Customer
            {
                Segment = CustomerSegment.VIP,
                Orders = new List<Order>()
            };

            var result = _discountService.ApplyDiscount(customer, 100m);
            Assert.Equal(80m, result);
        }

        [Fact]
        public void Premium_Customer_With_More_Than_3_Orders_Gets_15_Percent_Discount()
        {
            var customer = new Customer
            {
                Segment = CustomerSegment.Premium,
                Orders = new List<Order> { new(), new(), new(), new() }
            };

            var result = _discountService.ApplyDiscount(customer, 200m);
            Assert.Equal(170m, result);
        }

        [Fact]
        public void Regular_Customer_Gets_No_Discount()
        {
            var customer = new Customer
            {
                Segment = CustomerSegment.Regular,
                Orders = new List<Order>()
            };

            var result = _discountService.ApplyDiscount(customer, 50m);
            Assert.Equal(50m, result);
        }
    }

}
