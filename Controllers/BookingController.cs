using System.Diagnostics;
using MySql.Data.MySqlClient;
using QLThanhvien_Web.Models;
using QLThanhvien_Web.Services;

using Microsoft.AspNetCore.Mvc;

namespace QLThanhvien_Web.Controllers
{
    public class BookingController : Controller
    {

        private readonly DbConnectionHelper _db;
        public BookingController(DbConnectionHelper db)
        {
            _db = db;
        }

        public List<MemberHistory> GetMemberHistoryByMemberId(string memberId)
        {
            var members = new List<MemberHistory>();
            using var conn = _db.GetConnection();
            conn.Open();
            string query = "select * from members where member_id = @memberId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@memberId", memberId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var member = new MemberHistory
                {
                    member_id = reader["member_id"].ToString(),
                    device_id = reader["device_id"].ToString(),
                    reservation_id = reader["reservation_id"].ToString(),
                    violation_id = reader["violation_id"].ToString()
                };
                members.Add(member);
            }
            return members;
        }

        public List<Device> GetDeviceById(string deviceId)
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
                var dv = new Device
                {
                    device_id = reader["device_id"].ToString(),
                    device_name = reader["device_name"].ToString(),
                    device_type = reader["device_type"].ToString(),
                    status = reader["status"].ToString()
                };
                devices.Add(dv);
            }
            return devices;
        }

        public List<Device> GetAllDevice()
        {
            var devices = new List<Device>();
            using var conn = _db.GetConnection();
            conn.Open();
            string query = "Select * from devices";
            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var device = new Device
                {
                    device_id = reader["device_id"].ToString(),
                    device_name = reader["device_name"].ToString(),
                    device_type = reader["device_type"].ToString(),
                    status = reader["status"].ToString()
                };
                devices.Add(device);
            }
            return devices;
        }

        public List<Device> GetDevicesByInput(string input)
        {

            // If input is null or empty, return all devices
            if (string.IsNullOrEmpty(input))
            {
                return GetAllDevice();
            }
            var devices = new List<Device>();
            using var conn = _db.GetConnection();
            conn.Open();
            string query = @"SELECT * FROM devices 
                     WHERE device_name LIKE @input 
                     OR device_type LIKE @input";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@input", $"%{input}%"); // Use LIKE for partial matching
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var device = new Device
                {
                    device_id = reader["device_id"].ToString(),
                    device_name = reader["device_name"].ToString(),
                    device_type = reader["device_type"].ToString(),
                    status = reader["status"].ToString()
                };
                devices.Add(device);
            }
            return devices;
        }


        public void UpdateDeviceStatus(string deviceId)
        {

            using var conn = _db.GetConnection();
            conn.Open();
            string query = "UPDATE devices SET status = @newStatus WHERE device_id = @deviceId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@newStatus", "Được đặt chỗ");
            cmd.Parameters.AddWithValue("@deviceId", deviceId);
            cmd.ExecuteNonQuery();
        }

        public List<Reservation> GetReservationByMemberId(string memberId)
        {
            var reservations = new List<Reservation>();
            using var conn = _db.GetConnection();
            conn.Open();
            string query = "select * from reservations where member_id = @memberId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@memberId", memberId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var reservation = new Reservation
                {
                    reservation_id = reader["reservation_id"].ToString(),
                    member_id = reader["member_id"].ToString(),
                    device_id = reader["device_id"].ToString(),
                    reservation_date = Convert.ToDateTime(reader["reservation_date"]),
                    borrowed_date = Convert.ToDateTime(reader["borrowed_date"]),
                    returned_date = Convert.ToDateTime(reader["returned_date"]),
                    status = reader["status"].ToString()
                };
                reservations.Add(reservation);
            }
            return reservations;
        }

        private string GenerateReservationId()
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();

                // Query to get the latest reservation ID
                string query = "SELECT reservation_id FROM reservations WHERE reservation_id LIKE 'R%' ORDER BY reservation_id DESC LIMIT 1";
                using var cmd = new MySqlCommand(query, conn);
                var latestId = cmd.ExecuteScalar()?.ToString();

                // Generate the next ID
                if (string.IsNullOrEmpty(latestId))
                {
                    return "R001"; // Start with R001 if no reservations exist
                }

                // Extract the numeric part and increment it
                if (!int.TryParse(latestId.Substring(1), out int numericPart))
                {
                    throw new FormatException($"Invalid reservation_id format: {latestId}");
                }

                return $"R{(numericPart + 1).ToString("D3")}";
            }
            catch (Exception ex)
            {
                // Log the error (replace with your logging mechanism)
                Console.WriteLine($"Error generating reservation ID: {ex.Message}");
                throw;
            }
        }




        public void AddReservation (string deviceId, string reservationDate, string borrowedDate, string returnedDate)
        {
            string reservationId = GenerateReservationId(); // Auto-generate the ID
            string memberId = GetLoggedInUserId(); // Get logged in userId
            using var conn = _db.GetConnection();
            conn.Open();
            string query = @"INSERT INTO reservations 
                     (reservation_id, member_id, device_id, reservation_date, borrowed_date, returned_date, status) 
                     VALUES 
                     (@reservationId, @memberId, @deviceId, @reservationDate, @borrowedDate, @returnedDate, @status)";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@reservationId", reservationId);
            cmd.Parameters.AddWithValue("@memberId", memberId);
            cmd.Parameters.AddWithValue("@deviceId", deviceId);
            cmd.Parameters.AddWithValue("@reservationDate", Convert.ToDateTime(reservationDate));
            cmd.Parameters.AddWithValue("@borrowedDate", string.IsNullOrEmpty(borrowedDate) ? (object)DBNull.Value : Convert.ToDateTime(borrowedDate));
            cmd.Parameters.AddWithValue("@returnedDate", string.IsNullOrEmpty(returnedDate) ? (object)DBNull.Value : Convert.ToDateTime(returnedDate));
            cmd.Parameters.AddWithValue("@status", "Chờ duyệt");
            cmd.ExecuteNonQuery();
        }

        public string GetDeviceNameByReservation(Reservation reservation)
        {
            if (reservation == null || string.IsNullOrEmpty(reservation.device_id))
            {
                throw new ArgumentException("Invalid reservation or device_id.");
            }

            var devices = GetDeviceById(reservation.device_id);
            if (devices != null && devices.Count > 0)
            {
                return devices.First().device_name; // Assuming device_id is unique
            }

            return "Device not found";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public string GetLoggedInUserId()
        {
            return Request.Cookies["account_id"];
        }

        [HttpGet]
        public IActionResult SearchDevices(string input)
        {
            var devices = GetDevicesByInput(input);
            return Json(devices); // Return the devices as JSON for testing
        }


        public IActionResult Booking_device()
        {
            var ListAllDevice = GetAllDevice();
            if (ListAllDevice == null)
            {
                ListAllDevice = new List<Device>(); // tránh null
            }
            return View(ListAllDevice);
        }

        [HttpPost]
        public IActionResult RetrieveResData([FromBody] Reservation reservation)
        {
            try
            {
                // Add the reservation to the database
                AddReservation(
                    reservation.device_id,
                    reservation.reservation_date?.ToString(),
                    reservation.borrowed_date?.ToString(),
                    reservation.returned_date?.ToString()
                );
                UpdateDeviceStatus(reservation.device_id);
                // Return a success response
                return Json(new { success = true, message = "Reservation added successfully" });
            }
            catch (Exception ex)
            {
                // Return an error response
                return Json(new { success = false, message = ex.Message});
            }
        }



        public IActionResult Booking_history()
        {
            var info = GetLoggedInUserId();
            var memberList = GetReservationByMemberId(info);

            // Add device names to the ViewBag
            var deviceNames = memberList.ToDictionary(
                reservation => reservation.reservation_id,
                reservation => GetDeviceNameByReservation(reservation)
            );

            ViewBag.DeviceNames = deviceNames;

            return View(memberList);
        }
    }
}
