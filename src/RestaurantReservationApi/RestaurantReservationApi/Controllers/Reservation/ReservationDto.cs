namespace RestaurantReservationApi.Controllers.Reservation
{
    public sealed class ReservationDto
    {
        public string? Name { get; set; }

        public string? At { get; set; }

        public string? Email { get; set; }

        public int Quantity { get; set; }
    }
}
