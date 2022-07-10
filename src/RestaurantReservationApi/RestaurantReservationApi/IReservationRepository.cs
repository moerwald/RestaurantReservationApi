using RestaurantReservationApi.Controllers.Reservation;

namespace RestaurantReservationApi
{
    public interface IReservationRepository
    {
        Task Create(Reservation reservation);
    }
}
