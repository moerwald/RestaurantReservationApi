namespace RestaurantReservationApi.Controllers.Reservation
{
    public sealed record Reservation(string? Name, string? At, string? Email, int Quantity);
}
