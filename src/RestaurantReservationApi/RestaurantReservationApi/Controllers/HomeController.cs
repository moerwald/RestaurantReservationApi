using Microsoft.AspNetCore.Mvc;

namespace RestaurantReservationApi.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        public IActionResult Get()
        {
            return Ok(new { message = "Hello, World" });
        }
    }
}
