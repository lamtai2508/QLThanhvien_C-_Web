using Microsoft.AspNetCore.Mvc;
using QLThanhvien_Web.Models;

namespace QLThanhvien_Web.Controllers
{
    public class ChangePasswordController : Controller
    {
        [HttpPost]
        public IActionResult ForgotPassword(Account account, string newPassword, string confirmPassword)
        {
            if(string.IsNullOrEmpty(account.password) ||
               string.IsNullOrEmpty(newPassword) ||
               string.IsNullOrEmpty(confirmPassword)
                )
            {
                ViewBag.ErrorMessage = "không được để trống";
                return View();
            }
            return View();
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
