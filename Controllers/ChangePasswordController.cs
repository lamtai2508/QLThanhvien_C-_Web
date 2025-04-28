using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using QLThanhvien_Web.Models;
using QLThanhvien_Web.Services;

namespace QLThanhvien_Web.Controllers
{
    public class ChangePasswordController : Controller
    {
        private readonly DbConnectionHelper _db;
        public ChangePasswordController(DbConnectionHelper db)
        {
            _db = db;   
        }
        //Kiểm tra đã nhập đúng mật khẩu cũ hay chưa
        public bool CheckOldPassword(string AccountId, string AccountPassword )
        {
            using var conn = _db.GetConnection();
            conn.Open();
            string query = "select 1 from account where account_id = @AccountId and password = @AccountPassword";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@AccountId", AccountId);
            cmd.Parameters.AddWithValue("@AccountPassword", AccountPassword);
            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }
        //Hàm thay đổi mật khẩu mới
        public bool ChangeNewPassword(string AccountId, string newPassword)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            string query = "Update account set password = @newPassword where account_id = @AccountId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@AccountId", AccountId);
            cmd.Parameters.AddWithValue("@NewPassword", newPassword);

            int rowsEfftected = cmd.ExecuteNonQuery();
            return rowsEfftected > 0;
            
        }
        [HttpPost]
        public IActionResult ChangePassword(Account account, string newPassword, string confirmPassword)
        {
            var accountID = Request.Cookies["account_id"];
            if(string.IsNullOrEmpty(account.password) ||
               string.IsNullOrEmpty(newPassword) ||
               string.IsNullOrEmpty(confirmPassword)
                )
            {
                ViewBag.ErrorMessage = "không được để trống";
                return View();
            }
            if (!CheckOldPassword(accountID, account.password))
            {
                ViewBag.ErrorMessage = "Mật khẩu cũ không đúng";
                return View();
            }
            else
            {
                if (newPassword.Equals(confirmPassword))
                {
                    ChangeNewPassword(accountID, newPassword);
                    return RedirectToAction("Login","Login");
                }
                else
                {
                    ViewBag.ErrorMessage = "Xác nhận mật khẩu không đúng";
                    return View();
                }
            }
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}
