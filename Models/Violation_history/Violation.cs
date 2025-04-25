namespace QLThanhvien_Web.Models
{
    public class Violation
    {
        public string? violation_id { get; set; }
        public string? member_id { get; set; }
        public string? violation_type { get; set; }
        public string? penalty { get; set; }
        public DateTime? violation_date { get; set; }
        public DateTime? block_date { get; set; }
        public DateTime? unblock_date { get; set; }
        public string? status { get; set; } = "hoạt động";

    }
}
