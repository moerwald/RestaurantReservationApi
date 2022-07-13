namespace RestaurantReservationApi.Controllers.Reservation
{
    public sealed record Reservation(string? Name, DateTime At, string? Email, int Quantity);
}
