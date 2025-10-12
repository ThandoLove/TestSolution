namespace OutlookBlazorTestApp2.Data.Models.Auth
{
    
    
        public class AuthResult
    {
           
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? ErrorMessage { get; set; }
        public UserProfile? User { get; set; }
    }
}
    

