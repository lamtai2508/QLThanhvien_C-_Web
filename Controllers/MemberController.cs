using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using QLThanhvien_Web.Models;
using QLThanhvien_Web.Services;

namespace QLThanhvien_Web.Controllers
{
    public class MemberController : Controller
    {

        private readonly DbConnectionHelper _db;

        public MemberController(DbConnectionHelper db)
        {
            _db = db;
        }
        //Lấy thông tin của member theo Id
        public List<Member> GetMemberById(string memberId)
        {
            var members = new List<Member>();
            using var conn = _db.GetConnection();
            conn.Open();

            string query = "select * from members where member_id = @memberId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@memberId", memberId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read()) 
            {
                var mb = new Member
                {
                    member_id = reader["member_id"].ToString(),
                    full_name = reader["full_name"].ToString(),
                    gender = reader["gender"].ToString(),
                    number_phone = reader["number_phone"].ToString(),
                    dob = Convert.ToDateTime(reader["dob"]),
                    email = reader["email"].ToString(),
                    status = reader["status"].ToString()
                };
                members.Add(mb);
            }
            return members;
        }
        // Chính sửa thông tin
        public bool UpdateMember(Member mb)
        {
            var members = new List<Member>();
            using var conn = _db.GetConnection();
            conn.Open();
            string query = "UPDATE Members " +
                    "SET full_name = @full_name," +
                    "gender = @gender," +
                    "number_phone = @number_phone," +
                    "dob = @dob," +
                    "email = @email," +
                "WHERE member_id = @member_id;";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@full_name", mb.full_name);
            cmd.Parameters.AddWithValue("@gender", mb.gender);
            cmd.Parameters.AddWithValue("@number_phone", mb.number_phone);
            cmd.Parameters.AddWithValue("@dob", mb.dob);
            cmd.Parameters.AddWithValue("@email", mb.email);
            cmd.Parameters.AddWithValue("@member_id", mb.member_id);

            int rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected > 0;
        }
        public IActionResult Profile()
        {
            var accountId = Request.Cookies["account_id"]; //Lấy account_id từ cookie
            if (string.IsNullOrEmpty(accountId))
            {
                return RedirectToAction("Login", "Login"); // nếu cookie null, trả về trang đăng nhập
            }
            var info = GetMemberById(accountId); 
            return View(info);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
