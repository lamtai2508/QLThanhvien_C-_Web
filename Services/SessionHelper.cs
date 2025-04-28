namespace QLThanhvien_Web.Services
{
    public static class SessionHelper
    {
        private static IHttpContextAccessor _httpContextAccessor;

        // Cấu hình IHttpContextAccessor
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Lưu thông tin vào session
        public static void SetSession(string key, object value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, value.ToString());
        }

        // Lấy thông tin từ session
        public static string GetSession(string key)
        {
            return _httpContextAccessor.HttpContext.Session.GetString(key);
        }

        // Kiểm tra xem session có tồn tại hay không
        public static bool IsSessionActive(string key)
        {
            return _httpContextAccessor.HttpContext.Session.GetString(key) != null;
        }

        // Xóa thông tin khỏi session
        public static void RemoveSession(string key)
        {
            _httpContextAccessor.HttpContext.Session.Remove(key);
        }

        // Xóa toàn bộ session
        public static void ClearSession()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }
    }

}
