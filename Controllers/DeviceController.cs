using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using QLThanhvien_Web.Models;
using QLThanhvien_Web.Services;

namespace QLThanhvien_Web.Controllers
{
    public class DeviceController : Controller
    {
        private readonly DbConnectionHelper _db;
        public DeviceController(DbConnectionHelper db)
        {
            _db = db;
        }
        public List<Device> GetDevicesById(string deviceId)
        {
            var devices = new List<Device>();
            using var conn = _db.GetConnection();
            conn.Open();
            string query = "select * from devices where device_id = @deviceId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@deviceId", deviceId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var device = new Device
                {
                    device_id = reader["device_id"].ToString(),
                    device_name = reader["device_name"].ToString(),
                    device_type = reader["device_type"].ToString(),
                    status = reader["device_status"].ToString()
                };
                devices.Add(device);
            }
            return devices;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
