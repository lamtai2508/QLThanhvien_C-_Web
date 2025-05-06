using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using QLThanhvien_Web.Models;
using QLThanhvien_Web.Services;

namespace QLThanhvien_Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly DbConnectionHelper _db;
        public LoginController(DbConnectionHelper db) 
        {
            _db = db;
        }
        //Kiểm tra tài khoản có tồn tại hay không
        public string? GetAccountId(string AccountId)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            string query = "select account_id from Account where account_id = @AccountId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@AccountId", AccountId);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader["account_id"].ToString(); //trả về nếu tìm thấy
            }
            // không tìm thấy
            return null;
        }
        //Kiểm tra mật khẩu
        public bool CheckPassword(string AccountId, string Password)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            // Trả về nếu tồn tại 1 dữ liệu
            string query = "select 1 from account where account_id = @AccountId and password = @Password";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@AccountId", AccountId);
            cmd.Parameters.AddWithValue("@Password", Password);
            using var reader = cmd.ExecuteReader();
            return reader.Read(); // trả về true
        }

        // Lấy status member bằng account_id
        public string? GetStatusMember(string memberId)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            string query = "select status from members where member_id = @memberId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@memberId", memberId);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader["status"].ToString();
            }
            return null;
        }

        [HttpPost]
        public IActionResult Login(string account_id, string password)
        {
            var account = GetAccountId(account_id);

            // Báo lỗi khi để trống
            if (account_id == null || password == null)
            {
                ViewBag.ErrorMessage = "Không được để trống";
                return View();
            }
            // báo lỗi khi mã thành viên không tồn tại
            if (account == null)
            {
                ViewBag.ErrorMessage = "Mã thành viên không tồn tại!!";
                return View();
            }
            //đăng nhập thành công và lưu thông tin vào cookie
            if (CheckPassword(account_id, password))
            {
                if (GetStatusMember(account_id)?.ToLower() == "khóa vĩnh viễn")
                {
                    ViewBag.ErrorMessage = "Tài khoản thành viên đã bị khóa, không thể đăng nhập";
                    return View();
                }
                else { 
                    Response.Cookies.Append("account_id", account_id, new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.AddHours(1)
                    });
                    return RedirectToAction("Profile", "Member"); // chuyển hướng tới Profile
                }
            }

            //báo lỗi sai mật khẩu
            else
            {
                ViewBag.ErrorMessage = "Mật khẩu không chính xác!!";
                return View();
            }
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
    }
}
