using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace RestaurantReservationApi.Tests
{
    public class HomeTests
    {
        [Fact]
        public async Task HomeIsOk()
        {
            using var factory = new RestaurantApiFactory();
            var client = factory.CreateClient();

            var response = await client.GetAsync("");

            Assert.True(response.IsSuccessStatusCode, $"Actual status code: {response.StatusCode}");
        }

        [Fact]
        public async Task HomeReturnsJson()
        {
            using var factory = new RestaurantApiFactory();
            var client = factory.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, "");
            request.Headers.Accept.ParseAdd("application/json");
            var response = await client.SendAsync(request);

            Assert.True(response.IsSuccessStatusCode, $"Actual status code: {response.StatusCode}");
            Assert.Equal("application/json", response?.Content?.Headers?.ContentType?.MediaType);
        }
    }
}
