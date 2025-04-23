using Microsoft.AspNetCore.Mvc;

namespace QLThanhvien_Web.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Booking_device()
        {
            return View();
        }

        public IActionResult Booking_history()
        {
            return View();
        }
    }
}
