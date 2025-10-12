namespace OutlookBlazorTestApp2.Data.Models.Auth
{
    public class UserProfile
    {
        public string? UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // List of role names assigned to the user
        public List<string> Roles { get; set; } = new List<string>();

        // Last login timestamp
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
    }
}
