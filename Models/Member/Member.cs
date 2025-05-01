namespace QLThanhvien_Web.Models
{
    public class Member
    {
        public string? member_id { get; set; } 
        public string? full_name { get; set; } 
        public string? gender { get; set; } 
        public string? number_phone { get; set; } 
        public DateTime? dob { get; set; }
        public string? email { get; set; } 
        public string? status { get; set; } = "Đang hoạt động"; 
        
    }
}
