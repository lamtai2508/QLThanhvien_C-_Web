namespace QLThanhvien_Web.Models
{
    public class Reservation
    {
        public string? reservation_id { get; set; }
        public string? member_id { get; set; }
        public string? device_id { get; set; }
        public DateTime? reservation_date { get; set; }
        public DateTime? borrowed_date { get; set; }
        public DateTime? returned_date { get; set; }
        public string? status { get; set; } = "hoạt động";
        public Device Device { get; set; } = new Device();
        // set another method to get device name with device id

    }
}
