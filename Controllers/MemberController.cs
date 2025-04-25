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
        public IActionResult Profile()
        {
            var accountId = Request.Cookies["account_id"];
            if (string.IsNullOrEmpty(accountId))
            {
                return RedirectToAction("Login", "Login");
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
