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
                    return_date = reader["return_date"] == DBNull.Value
                      ? (DateTime?) null
                      : Convert.ToDateTime(reader["return_date"]),
                    status = reader["status"].ToString(),
                };
                DR.Add(deviceReturn);
            }
            return DR;
        }
        public bool updateDeviceReturn(string deviceId, DateTime returnDate, string status)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            // Kiểm tra bản ghi mượn gần nhất chưa trả
            string checkQuery = @"
            SELECT COUNT(*) 
            FROM borroweddevices 
            WHERE device_id = @deviceId AND return_date IS NULL";
            var checkCmd = new MySqlCommand(checkQuery, conn);
            checkCmd.Parameters.AddWithValue("@deviceId", deviceId);

            var result = Convert.ToInt32(checkCmd.ExecuteScalar());

            // Nếu không có bản ghi chưa trả, nghĩa là thiết bị đã được trả rồi
            if (result == 0)
            {
                return false;
            }

            // Cập nhật bản ghi mượn chưa được trả
            string updateQuery = @"
            UPDATE borroweddevices 
            SET return_date = @return_date, status = @status 
            WHERE device_id = @deviceId AND return_date IS NULL";
            var updateCmd = new MySqlCommand(updateQuery, conn);
            updateCmd.Parameters.AddWithValue("@deviceId", deviceId);
            updateCmd.Parameters.AddWithValue("@return_date", returnDate);
            updateCmd.Parameters.AddWithValue("@status", status);

            int rowsAffected = updateCmd.ExecuteNonQuery();
            return rowsAffected > 0;
        }

        public bool updateDeviceStatus(string deviceId, string status)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            string query = "UPDATE devices SET status = @status WHERE device_id = @deviceId";
            var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@deviceId", deviceId);
            cmd.Parameters.AddWithValue("@status", @status);
            int rowsEffect = cmd.ExecuteNonQuery();
            return rowsEffect > 0;
        }
        [HttpPost]
        public IActionResult DeviceReturn(string device_id, DateTime return_date)
        {
            var accountid = Request.Cookies["account_id"];
            Response.Cookies.Append("device_id", device_id, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddHours(1)
            });

            //var deviceId = Request.Cookies["device_id"];
            var deviceId = device_id;
            if (string.IsNullOrEmpty(deviceId))
            {
                return Json(new { success = false, message = "Không có thông tin thiết bị trong cookie." });
            }

            // Cập nhật thiết bị trả về
            if (!updateDeviceReturn(deviceId, return_date, "Đã trả lại"))
            {
                return Json(new { success = false, message = "Thiết bị đã được trả lại rồi." });
            }
            else
            {
                updateDeviceStatus(deviceId, "Có sẵn");
                return Json(new { success = true, message = "Thiết bị đã được trả lại thành công!" });
            }
        }
        [HttpGet]
        public IActionResult DeviceReturn()
        {
            var accountId = Request.Cookies["account_id"];
            var info = GetDeviceReturnById(accountId);
            return View(info);
        }
    }
}
