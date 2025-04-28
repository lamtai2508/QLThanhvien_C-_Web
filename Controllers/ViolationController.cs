using System.Diagnostics;
using MySql.Data.MySqlClient;
using QLThanhvien_Web.Models;
using QLThanhvien_Web.Services;

using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;

namespace QLThanhvien_Web.Controllers
{
    public class ViolationController : Controller
    {
        private readonly DbConnectionHelper _db;
        public ViolationController(DbConnectionHelper db)
        {
            _db = db;
        }
        public List<Violation> GetViolationByMemberId(string memberId)
        {
            var violations = new List<Violation>();
            using var conn = _db.GetConnection();
            conn.Open();
            string query = "select * from violations where member_id = @memberId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@memberId", memberId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var vl = new Violation
                {
                    violation_id = reader["violation_id"].ToString(),
                    member_id = reader["member_id"].ToString(),
                    violation_type = reader["violation_type"].ToString(),
                    penalty = reader["penalty"].ToString(),
                    violation_date = Convert.ToDateTime(reader["violation_date"]),
                    block_date = Convert.ToDateTime(reader["block_date"]),
                    unblock_date = Convert.ToDateTime(reader["unblock_date"]),
                    status = reader["status"].ToString()
                };
                violations.Add(vl);
            }
            return violations;
        }

        public string GetLoggedInUserId()
        {
            return Request.Cookies["account_id"];
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Violations_history()
        {
            var memberId = GetLoggedInUserId();
            var info = GetViolationByMemberId(memberId);
            return View(info);
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
