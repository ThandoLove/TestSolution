namespace OutlookBlazorTestApp2.Data.Models.Auth
{
    
        public class AuthState
        {
            public bool IsAuthenticated { get; set; }
            public string? UserId { get; set; }
            public string? UserName { get; set; }
            public string? Email { get; set; }
            public string? Token { get; set; }
            public List<string> Roles { get; set; } = new();
            public DateTime? TokenExpiry { get; set; }
        }
    
}
