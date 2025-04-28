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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Booking_device()
        {
            return View();
        }

        public IActionResult Booking_history()
        {
            var info = GetReservationByMemberId("TV113");
            return View(info);
        }
    }
}
