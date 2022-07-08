using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservationApi.Tests
{
    public class HomeTests
    {
        [Fact]
        public async Task HomeIsOk()
        {
            using var factory = new WebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            var response = await client.GetAsync("").ConfigureAwait(false);

            Assert.True(response.IsSuccessStatusCode, $"Actual status code: {response.StatusCode}");
        }
    }
}
