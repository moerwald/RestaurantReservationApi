using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestaurantReservationApi.Tests
{
    public class ReservationTests
    {
        [Fact]
        public async Task PostValidatReservation()
        {
            var response = await PostReservationAsync(
                new
                {
                    name = "John Doe",
                    email = "john.doe@gmail.com",
                    quantity = 2,
                    date = "2023-03-10 19:40"
                });

            Assert.False(response.IsSuccessStatusCode,
                $"Actual status code: {response.StatusCode}");
        }

        private static async Task<HttpResponseMessage> PostReservationAsync(object content)
        {
            using var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();

            var c = new StringContent(JsonSerializer.Serialize(content));
            c.Headers!.ContentType!.MediaType = "application/json";

            return await client.PostAsync("reservations", c);
        }
    }


}
