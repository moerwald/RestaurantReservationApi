using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RestaurantReservationApi.Controllers.Reservation;

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
                    at = "2023-03-10 19:40"
                });

            Assert.True(response.IsSuccessStatusCode,
                $"Actual status code: {response.StatusCode}");
        }

        [Theory]
        [InlineData("2023-11-24 19:00", "juliad@example.com", "Julia Domna", 5)]
        [InlineData("2024-02-13 18:15", "x@example.com", "Xenia Ng", 9)]
        public async Task PostValidReservationWhenDatabaseIsEmpty(
            string at,
            string email,
            string name,
            int quantity)
        {
            var db = new FakeDatabase();
            var sut = new ReservationController(db);

            var dto = new ReservationDto()
            {
                At = at,
                Email = email,
                Name = name,
                Quantity = quantity
            };

            await sut.Post(dto);

            var expected = new Reservation(dto.Name, DateTime.Parse(dto.At, CultureInfo.InvariantCulture), dto.Email,
                dto.Quantity);

            Assert.Contains(expected, db);

        }

        private static async Task<HttpResponseMessage> PostReservationAsync(object content)
        {
            await using var factory = new RestaurantApiFactory();
            var client = factory.CreateClient();

            var c = new StringContent(JsonSerializer.Serialize(content));
            c.Headers!.ContentType!.MediaType = "application/json";

            return await client.PostAsync("reservations", c);
        }
    }


}
