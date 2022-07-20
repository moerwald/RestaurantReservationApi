using RestaurantReservationApi.Controllers.Reservation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservationApi.Tests
{
    public class FakeDatabase : Collection<Reservation>, IReservationRepository
    {
        public Task CreateAsync(Reservation reservation)
        {
            Add(reservation);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Reservation>> ReadReservationsAsync(DateTime dateTime) =>
            Task.FromResult(this.Where(r => r.At.Date == dateTime.Date));
    }
}
