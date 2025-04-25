using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using QLThanhvien_Web.Models;
using QLThanhvien_Web.Services;
using System.Reflection;

namespace QLThanhvien_Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly DbConnectionHelper _db;
        
        public RegisterController(DbConnectionHelper db)
        {
            _db = db;
        }
        // Kiểm tra xem Id đã tồn tại hay chưa
        public bool CheckMemberId(string MemberId)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            string query = "select 1 from members where member_id = @MemberId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MemberId", MemberId);
            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }
        //Đăng ký 
        //Thêm thông tin member
        public bool RegisterMember(Member mb)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            string query = "INSERT INTO members (member_id, full_name, gender, number_phone, dob, email, status) " +
                "VALUES(@member_id, @full_name, @gender, @number_phone, @dob, @email, @status)";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@member_id", mb.member_id);
            cmd.Parameters.AddWithValue("@full_name", mb.full_name);
            cmd.Parameters.AddWithValue("@gender", mb.gender);
            cmd.Parameters.AddWithValue("@number_phone", mb.number_phone);
            cmd.Parameters.AddWithValue("@dob", mb.dob);
            cmd.Parameters.AddWithValue("@email", mb.email);
            cmd.Parameters.AddWithValue("@status", mb.status);

            int rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected > 0;
        }
        //Thêm thông tin account
        public bool RegisterAccount(Account account)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            string query = "INSERT INTO account(account_id,password) " +
                "VALUES(@account_id,@password)";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@account_id", account.account_id);
            cmd.Parameters.AddWithValue("@password", account.password);

            int rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected > 0;
        }
        [HttpPost]
        public IActionResult Register(Member mb, Account account)
        {
            //Báo lỗi khi để ô trống
            if (string.IsNullOrEmpty(mb.member_id) ||
                string.IsNullOrEmpty(mb.full_name) ||
                string.IsNullOrEmpty(mb.gender) ||
                string.IsNullOrEmpty(mb.number_phone) ||
                string.IsNullOrEmpty(mb.email) ||
                mb.dob == null||
                string.IsNullOrEmpty(account.password))
            {
                ViewBag.ErrorMessage = "không được để trống";
                return View();
            }
            //báo lỗi khi tồn tại member
            if (CheckMemberId(mb.member_id))
            {
                ViewBag.ErrorMessage = "Mã thành viên đã tồn tại!!";
                return View();
            }
            //Đăng ký member
            if (RegisterMember(mb))
            {
                account.account_id = mb.member_id;
                if (RegisterAccount(account))
                {
                    ViewBag.Success = "Đăng ký thành công";
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    ViewBag.ErrorMessage = "Đăng ký tài khoản thất bại";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Đăng ký thành viên thất bại";
            }
                return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}
