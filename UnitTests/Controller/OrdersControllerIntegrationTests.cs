using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using OrderManagement;
using OrderManagement.Models;
using Xunit;
namespace OrderManagement.UnitTests
{
    public class OrdersControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public OrdersControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateOrder_Returns_Success_And_Calculates_Discount()
        {
            int customerId = 1;
            decimal baseAmount = 100m;

            var response = await _client.PostAsync($"/api/orders/create?customerId={customerId}&baseAmount={baseAmount}", null);
            response.EnsureSuccessStatusCode();

            var order = await response.Content.ReadFromJsonAsync<OrderResponse>();
            Assert.NotNull(order);
            Assert.True(order.TotalAmount <= baseAmount);
            Assert.Equal("Pending", order.Status);
        }
    }

    public class OrderResponse
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
