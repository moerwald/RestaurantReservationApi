using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        [Theory]
        [InlineData(null, "a@gmail.com", "2022-03-10 19:00", 1)]
        [InlineData("John Doe", null, "2022-03-10 19:00", 1)]
        [InlineData("John Doe", "a@gmail.com", "not a date", 1)]
        [InlineData("John Doe", "a@gmail.com", "2022-03-10 19:00", 0)]
        [InlineData("John Doe", "a@gmail.com", "2022-03-10 19:00", -1)]
        public async Task PostInvalidInputData(
            string name,
            string email,
            string at,
            int quantity)
        {
            var response = await PostReservationAsync(new {name, email, at, quantity});
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
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
