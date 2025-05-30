using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using OrderManagement;
using OrderManagement.Models;
using Xunit;
namespace OrderManagement.UnitTests
{
    using System.Net.Http.Json;
    using Microsoft.AspNetCore.Mvc.Testing;
    using OrderManagement.Models;
    using OrderManagement.UnitTests.Infrastructure;
    using Xunit;

    public class OrdersControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public OrdersControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateOrder_ReturnsOrder_WithDiscountApplied()
        {
            // Arrange - assuming customer with Id=1 exists and is Premium with more than 3 orders
            int customerId = 1;
            decimal baseAmount = 100m;

            // Act
            var response = await _client.PostAsync($"/api/orders/create?customerId={customerId}&baseAmount={baseAmount}", null);
            response.EnsureSuccessStatusCode();

            var orderResponse = await response.Content.ReadFromJsonAsync<OrderResponse>();

            // Assert
            Assert.NotNull(orderResponse);
            Assert.True(orderResponse.TotalAmount <= baseAmount, "Discount should be applied");
            Assert.Equal("Pending", orderResponse.Status);
            Assert.Equal(customerId, orderResponse.CustomerId);
        }
    }
}
