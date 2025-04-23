namespace QLThanhvien_Web.Models
{
    public class Member
    {
        public string member_id { get; set; } = string.Empty;
        public string full_name { get; set; } = string.Empty;
        public string gender { get; set; } = string.Empty;
        public string number_phone { get; set; } = string.Empty;
        public DateTime? dob { get; set; }
        public string email { get; set; } = string.Empty;
        public string status { get; set; } = "hoạt động";
        
    }
}
