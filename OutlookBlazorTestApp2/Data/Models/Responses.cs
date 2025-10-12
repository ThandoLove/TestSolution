namespace OutlookBlazorTestApp2.Data.Models
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Message { get; set; } = string.Empty;
    }

    public class ApiError
    {
        public string Message { get; set; } = "";
        public List<string> Errors { get; set; } = new();
        public string ErrorCode { get; set; } = "";
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public ApiError()
        {
        }

        public ApiError(string message, List<string>? errors = null, string? errorCode = null)
        {
            Message = message;
            Errors = errors ?? new List<string>();
            ErrorCode = errorCode ?? "";
        }
    }

    public class SearchResponse<T>
    {
        public List<T> Results { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool HasMore { get; set; }
    }

    public class AutoLinkResponse
    {
        public bool MatchFound { get; set; }
        public CrmRecord? Record { get; set; }
        public string Message { get; set; } = "";
         
}

    public class AttachmentProcessResponse
    {
        public List<ProcessedAttachment> ProcessedAttachments { get; set; } = new();
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    public class ProcessedAttachment
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public bool Success { get; set; }
        public string Message { get; set; } = "";
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; } = "";
        public DateTime Expiry { get; set; }
        public UserProfile UserProfile { get; set; } = new();
        public List<string> Roles { get; set; } = new();
    }

    public class UserProfile
    {
        public string UserId { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
        public DateTime LastLogin { get; set; }
        public UserPreferences Preferences { get; set; } = new();
    }

    public class UserPreferences
    {
        public string DefaultView { get; set; } = "";
        public bool AutoLinkEmails { get; set; }
        public bool ShowActivityTimeline { get; set; }
        public string TimeZone { get; set; } = "";
        public string Language { get; set; } = "";
    }

    public class CrmRecord
    {
        public Guid Id { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? Summary { get; set; }
    }

    public class OperationResult<T>
    {
        public bool? Success { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public List<string> ValidationErrors { get; set; } = new();
        public int StatusCode { get; set; }
    }
}
