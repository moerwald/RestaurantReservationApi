using RestaurantReservationApi.Controllers.Reservation;

namespace RestaurantReservationApi.Tests;

public class ReservationTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void QuantityMustBePositive(int q)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Reservation(
                "mail@example.com",
                DateTime.Now,
                "John Doe",
                q));
    }
}