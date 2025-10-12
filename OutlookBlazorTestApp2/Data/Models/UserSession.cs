
namespace OutlookBlazorTestApp2.Data.Models
{
    public class UserSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AuthToken { get; set; } = "";
        public DateTime ExpiresAt { get; set; }
    }
}
