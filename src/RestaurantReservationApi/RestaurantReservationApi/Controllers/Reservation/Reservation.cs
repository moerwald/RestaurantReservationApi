using System.Xml;

namespace RestaurantReservationApi.Controllers.Reservation
{
    public sealed record Reservation
    {
        public string? Name { get; init; }
        public DateTime At { get; init; }
        public string? Email { get; init; }
        public int Quantity { get; init; }

        public Reservation(string? name, DateTime at, string? email, int quantity)
        {
            Name = name;
            At = at;
            Email = email;
            Quantity = quantity;
            if (quantity < 1)
                throw new ArgumentOutOfRangeException("Quantity must be greater 0");
        }
        
    }
        
        
    
}
