
using OrderManagement.Models;
using OrderManagement.Services;
using Xunit;
using System.Collections.Generic;

namespace OrderManagement.UnitTests.Model
{

    public class DiscountServiceTests
    {
        private readonly DiscountService _discountService = new();

        [Fact]
        public void PremiumCustomer_WithMoreThan3Orders_Gets5PercentDiscount()
        {
            var customer = new Customer
            {
                Segment = CustomerSegment.Premium,
                Orders = new List<Order> { new(), new(), new(), new() } // 4 orders
            };

            decimal baseAmount = 100m;
            decimal expected = 95m;

            var actual = _discountService.ApplyDiscount(customer, baseAmount);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PremiumCustomer_With3OrFewerOrders_GetsNoDiscount()
        {
            var customer = new Customer
            {
                Segment = CustomerSegment.Premium,
                Orders = new List<Order> { new(), new(), new() } // exactly 3 orders
            };

            decimal baseAmount = 100m;
            decimal expected = 100m;

            var actual = _discountService.ApplyDiscount(customer, baseAmount);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VIPCustomer_AlwaysGets10PercentDiscount()
        {
            var customer = new Customer
            {
                Segment = CustomerSegment.VIP,
                Orders = new List<Order>() // number of orders doesn’t matter for VIP
            };

            decimal baseAmount = 100m;
            decimal expected = 90m;

            var actual = _discountService.ApplyDiscount(customer, baseAmount);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void StandardCustomer_GetsNoDiscount()
        {
            var customer = new Customer
            {
                Segment = CustomerSegment.Regular,
                Orders = new List<Order>()
            };

            decimal baseAmount = 100m;
            decimal expected = 100m;

            var actual = _discountService.ApplyDiscount(customer, baseAmount);

            Assert.Equal(expected, actual);
        }
    }
}
