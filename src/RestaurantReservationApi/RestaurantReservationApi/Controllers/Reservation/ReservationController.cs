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

        public async Task<IActionResult> Post(ReservationDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (dto.Name is null || dto.Email is null || !DateTime.TryParse(dto.At, out _) || dto.Quantity <= 0)
            {
                return new BadRequestResult();
            }

            var quantitySum =
                (await _reservationRepository.ReadReservationsAsync(DateTime.Parse(dto.At))).Sum(r => r.Quantity);
            
            if (10 < quantitySum + dto.Quantity)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            await _reservationRepository
                .CreateAsync(new Reservation(dto.Name, DateTime.Parse(dto.At!, CultureInfo.InvariantCulture), dto.Email,
                    dto.Quantity)).ConfigureAwait(false);

            return new NoContentResult();
        }
    }
}