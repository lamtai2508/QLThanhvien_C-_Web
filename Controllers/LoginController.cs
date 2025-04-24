using Microsoft.AspNetCore.Mvc;

namespace QLThanhvien_Web.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
