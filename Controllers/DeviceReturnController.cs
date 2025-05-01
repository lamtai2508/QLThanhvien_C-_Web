using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using QLThanhvien_Web.Models;
using QLThanhvien_Web.Services;

namespace QLThanhvien_Web.Controllers
{
    public class DeviceReturnController : Controller
    {
        private readonly DbConnectionHelper _db;
        public DeviceReturnController(DbConnectionHelper db)
        {
            _db = db;
        }
        // Hàm lấy danh sách thiết bị mươn bởi memberId
        public List<DeviceReturn> GetDeviceReturnById(string memberId)
        {
            var DR= new List<DeviceReturn>();
            using var conn = _db.GetConnection();
            conn.Open();
            string query = "select * from borroweddevices where member_id = @memberId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@memberId", memberId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var deviceReturn = new DeviceReturn
                {
                    member_id = reader["member_id"].ToString(),
                    device_id = reader["device_id"].ToString(),
                    borrow_date = Convert.ToDateTime(reader["borrow_date"]),
                    due_date = Convert.ToDateTime(reader["due_date"]),
                    return_date = Convert.ToDateTime(reader["return_date"]),
                    status = reader["status"].ToString(),
                };
                DR.Add(deviceReturn);
            }
            return DR;
        }
        public IActionResult DeviceReturn()
        {
            var accountId = Request.Cookies["account_id"];
            var info = GetDeviceReturnById(accountId);
            return View(info);
        }
    }
}
