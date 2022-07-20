using System.Globalization;
using System.Net;
using System.Text.Json;
using RestaurantReservationApi.Controllers.Reservation;

namespace RestaurantReservationApi.Tests
{
    public class ReservationControllerTests
    {
        [Fact]
        public async Task PostValidateReservation()
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
        [InlineData("2024-03-18 17:30", "shli@example.org", "Shanghai Li", 9)]
        public async Task PostValidReservationWhenDatabaseIsEmpty(
            string at,
            string email,
            string name,
            int quantity)
        {
            var db = new FakeDatabase();
            var sut = new ReservationController(db);
            var dto = new ReservationDto
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
        
        [Fact]
        public async Task OverbookAttempt()
        {
            await using var service = new RestaurantApiFactory();
            await service.PostReservationAsync(new
            {
                at = "2022-03-18 17:30",
                email = "mars@example.edu",
                name = "Marina Seminova",
                quantity = 6
            });
            
            var response = await service.PostReservationAsync(new
            {
                at = "2022-03-18 17:30",
                email = "shli@example.org",
                name = "Shanghai Li",
                quantity = 5
            });

            Assert.Equal(
                HttpStatusCode.InternalServerError,
                response.StatusCode);

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
