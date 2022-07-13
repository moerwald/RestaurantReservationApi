using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantReservationApi.Controllers.Reservation
{
    [ApiController, Route("reservations")]
    public class ReservationController
    {
        private IReservationRepository _reservationRepository;

        public ReservationController(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task Post(ReservationDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            await _reservationRepository
                .Create(new Reservation(dto.Name, DateTime.Parse(dto.At!, CultureInfo.InvariantCulture), dto.Email,
                    dto.Quantity)).ConfigureAwait(false);
        }
    }
}