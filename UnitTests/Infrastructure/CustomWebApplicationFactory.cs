using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using OrderManagement;
using OrderManagement.Models;
using Xunit;
namespace OrderManagement.UnitTests.Infrastructure
{

    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            // Set content root to your app project folder
            builder.UseContentRoot("D:\\Data\\Task\\OrderManagement");
            return base.CreateHost(builder);
        }
    }

    public class OrdersControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public OrdersControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
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
            Assert.True(order.TotalAmount <= baseAmount); // discount applied
            Assert.Equal("Pending", order.Status);
        }

        
    }
}
