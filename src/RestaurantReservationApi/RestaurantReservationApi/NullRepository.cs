using RestaurantReservationApi.Controllers.Reservation;

namespace RestaurantReservationApi
{
    public class NullRepository : IReservationRepository
    {
        public Task Create(Reservation reservation) => Task.CompletedTask;
    }
}
