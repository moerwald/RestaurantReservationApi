using RestaurantReservationApi.Controllers.Reservation;

namespace RestaurantReservationApi
{
    public interface IReservationRepository
    {
        Task CreateAsync(Reservation reservation);
        Task<IEnumerable<Reservation>> ReadReservationsAsync(DateTime dateTime);
    }
}
