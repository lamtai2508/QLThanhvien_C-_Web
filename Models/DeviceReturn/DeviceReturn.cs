namespace QLThanhvien_Web.Models
{
    public class DeviceReturn
    {
        public string? member_id { get; set; }
        public string? device_id { get; set; }
        public DateTime? borrow_date { get; set; }
        public DateTime? due_date { get; set; }
        public DateTime? return_date { get; set; }
        public string? status { get; set; } = "Đang mượn";

    }
}
