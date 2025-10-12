
using Microsoft.JSInterop;
using System.Text.Json;

namespace OutlookBlazorTestApp2.Services
{
    // Step 1️⃣ — Define the Interface (Contract for Outlook operations)
    public interface IOutlookService
    {
        Task<OperationResult<EmailContext>> GetEmailContextAsync();
        Task<OperationResult<string>> GetEmailBodyAsync(string format = "text");
        Task<OperationResult<List<AttachmentInfo>>> GetAttachmentsAsync();
        Task<OperationResult<string>> GetSelectedTextAsync();
        Task<OperationResult<UserProfile>> GetUserProfileAsync();

        // ✨ New method for file attachments
        Task AttachFileAsync(string fileName, byte[] fileBytes);
    }

    // Step 2️⃣ — Implementation of the Outlook service logic
    public class OutlookService : IOutlookService
    {
        // These are dependency-injected components:
        // - IJSRuntime lets us call JavaScript from C#
        // - ILogger logs errors and events
        private readonly IJSRuntime _jsRuntime;
        private readonly ILogger<OutlookService> _logger;

        // ✅ Constructor initializes these dependencies.
        // No warning will appear because both are required and assigned.
        public OutlookService(IJSRuntime jsRuntime, ILogger<OutlookService> logger)
        {
            _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Step 3️⃣ — Attach a file to an Outlook email via JavaScript interop
        // This is the method from your smaller snippet.
        public async Task AttachFileAsync(string fileName, byte[] fileBytes)
        {
            try
            {
                string base64 = Convert.ToBase64String(fileBytes);
                await _jsRuntime.InvokeVoidAsync("attachFileToEmail", fileName, base64);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attaching file to email");
                throw; // rethrow for UI or error handling
            }
        }

        // ---------------- Core Outlook Interop Methods ----------------

        public async Task<OperationResult<EmailContext>> GetEmailContextAsync()
        {
            try
            {
                var result = await _jsRuntime.InvokeAsync<JsonElement>("officeInterop.getEmailContext");

                if (result.TryGetProperty("error", out var error))
                {
                    return new OperationResult<EmailContext>
                    {
                        Success = false,
                        ErrorMessage = error.GetString(),
                        StatusCode = 500
                    };
                }

                var emailContext = new EmailContext
                {
                    ItemId = result.TryGetProperty("itemId", out var itemId) ? itemId.GetString() ?? "" : "",
                    Subject = result.TryGetProperty("subject", out var subject) ? subject.GetString() ?? "" : "",
                    ReceivedTime = result.TryGetProperty("receivedTime", out var receivedTime) &&
                                   DateTime.TryParse(receivedTime.GetString(), out var rt) ? rt : DateTime.MinValue,
                    ConversationId = result.TryGetProperty("conversationId", out var convId) ? convId.GetString() ?? "" : "",
                    ConversationTopic = result.TryGetProperty("conversationTopic", out var convTopic) ? convTopic.GetString() ?? "" : "",
                    IsRead = result.TryGetProperty("isRead", out var isRead) && isRead.GetBoolean(),
                    Importance = result.TryGetProperty("importance", out var importance) ?
                                Enum.TryParse<ImportanceLevel>(importance.GetString(), true, out var imp) ? imp : ImportanceLevel.Normal :
                                ImportanceLevel.Normal
                };

                if (result.TryGetProperty("sender", out var senderElement) && senderElement.ValueKind == JsonValueKind.Object)
                {
                    emailContext.Sender = new EmailAddress
                    {
                        Name = senderElement.TryGetProperty("name", out var senderName) ? senderName.GetString() ?? "" : "",
                        Address = senderElement.TryGetProperty("address", out var senderAddress) ? senderAddress.GetString() ?? "" : ""
                    };
                }

                emailContext.ToRecipients = ParseRecipients(result, "toRecipients");
                emailContext.CcRecipients = ParseRecipients(result, "ccRecipients");
                emailContext.BccRecipients = ParseRecipients(result, "bccRecipients");
                emailContext.Attachments = ParseAttachments(result, "attachments");

                return new OperationResult<EmailContext>
                {
                    Success = true,
                    Data = emailContext,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting email context");
                return new OperationResult<EmailContext>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                };
            }
        }

        public async Task<OperationResult<string>> GetEmailBodyAsync(string format = "text")
        {
            try
            {
                var result = await _jsRuntime.InvokeAsync<JsonElement>("officeInterop.getEmailBody", format);

                if (result.TryGetProperty("error", out var error))
                {
                    return new OperationResult<string>
                    {
                        Success = false,
                        ErrorMessage = error.GetString(),
                        StatusCode = 500
                    };
                }

                var body = result.TryGetProperty("body", out var bodyElement) ? bodyElement.GetString() ?? "" : "";

                return new OperationResult<string>
                {
                    Success = true,
                    Data = body,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting email body");
                return new OperationResult<string>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                };
            }
        }

        public async Task<OperationResult<List<AttachmentInfo>>> GetAttachmentsAsync()
        {
            try
            {
                var result = await _jsRuntime.InvokeAsync<JsonElement>("officeInterop.getAttachments");

                if (result.TryGetProperty("error", out var error))
                {
                    return new OperationResult<List<AttachmentInfo>>
                    {
                        Success = false,
                        ErrorMessage = error.GetString(),
                        StatusCode = 500
                    };
                }

                var attachments = new List<AttachmentInfo>();

                if (result.TryGetProperty("attachments", out var attachmentsElement) &&
                    attachmentsElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var attachmentElement in attachmentsElement.EnumerateArray())
                    {
                        var attachment = new AttachmentInfo
                        {
                            Id = attachmentElement.TryGetProperty("id", out var id) ? id.GetString() ?? "" : "",
                            Name = attachmentElement.TryGetProperty("name", out var name) ? name.GetString() ?? "" : "",
                            Size = attachmentElement.TryGetProperty("size", out var size) ? size.GetInt32() : 0,
                            ContentType = attachmentElement.TryGetProperty("contentType", out var contentType) ? contentType.GetString() ?? "" : ""
                        };
                        attachments.Add(attachment);
                    }
                }

                return new OperationResult<List<AttachmentInfo>>
                {
                    Success = true,
                    Data = attachments,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting attachments");
                return new OperationResult<List<AttachmentInfo>>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                };
            }
        }

        public async Task<OperationResult<string>> GetSelectedTextAsync()
        {
            try
            {
                var result = await _jsRuntime.InvokeAsync<JsonElement>("officeInterop.getSelectedText");

                if (result.TryGetProperty("error", out var error))
                {
                    return new OperationResult<string>
                    {
                        Success = false,
                        ErrorMessage = error.GetString(),
                        StatusCode = 500
                    };
                }

                var text = result.TryGetProperty("text", out var textElement) ? textElement.GetString() ?? "" : "";

                return new OperationResult<string>
                {
                    Success = true,
                    Data = text,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting selected text");
                return new OperationResult<string>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                };
            }
        }

        public async Task<OperationResult<UserProfile>> GetUserProfileAsync()
        {
            try
            {
                var result = await _jsRuntime.InvokeAsync<JsonElement>("officeInterop.getUserProfile");

                if (result.TryGetProperty("error", out var error))
                {
                    return new OperationResult<UserProfile>
                    {
                        Success = false,
                        ErrorMessage = error.GetString(),
                        StatusCode = 500
                    };
                }

                var userProfile = new UserProfile
                {
                    DisplayName = result.TryGetProperty("displayName", out var displayName) ? displayName.GetString() ?? "" : "",
                    EmailAddress = result.TryGetProperty("emailAddress", out var email) ? email.GetString() ?? "" : "",
                    TimeZone = result.TryGetProperty("timeZone", out var timeZone) ? timeZone.GetString() ?? "" : ""
                };

                return new OperationResult<UserProfile>
                {
                    Success = true,
                    Data = userProfile,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile");
                return new OperationResult<UserProfile>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                };
            }
        }

        // Step 4️⃣ — Helper functions for parsing JSON into models
        private List<EmailAddress> ParseRecipients(JsonElement element, string propertyName)
        {
            var recipients = new List<EmailAddress>();

            if (element.TryGetProperty(propertyName, out var recipientsElement) &&
                recipientsElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var recipientElement in recipientsElement.EnumerateArray())
                {
                    var recipient = new EmailAddress
                    {
                        Name = recipientElement.TryGetProperty("name", out var name) ? name.GetString() ?? "" : "",
                        Address = recipientElement.TryGetProperty("address", out var address) ? address.GetString() ?? "" : ""
                    };
                    recipients.Add(recipient);
                }
            }

            return recipients;
        }

        private List<AttachmentInfo> ParseAttachments(JsonElement element, string propertyName)
        {
            var attachments = new List<AttachmentInfo>();

            if (element.TryGetProperty(propertyName, out var attachmentsElement) &&
                attachmentsElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var attachmentElement in attachmentsElement.EnumerateArray())
                {
                    var attachment = new AttachmentInfo
                    {
                        Id = attachmentElement.TryGetProperty("id", out var id) ? id.GetString() ?? "" : "",
                        Name = attachmentElement.TryGetProperty("name", out var name) ? name.GetString() ?? "" : "",
                        Size = attachmentElement.TryGetProperty("size", out var size) ? size.GetInt32() : 0,
                        ContentType = attachmentElement.TryGetProperty("contentType", out var contentType) ? contentType.GetString() ?? "" : ""
                    };
                    attachments.Add(attachment);
                }
            }

            return attachments;
        }
    }

    // ---------------- Models & Result Wrappers ----------------

    public class OperationResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public List<string> ValidationErrors { get; set; } = new();
        public int StatusCode { get; set; }
    }

    public class EmailContext
    {
        public string ItemId { get; set; } = "";
        public string Subject { get; set; } = "";
        public string Body { get; set; } = "";
        public DateTime ReceivedTime { get; set; }
        public EmailAddress Sender { get; set; } = new();
        public List<EmailAddress> ToRecipients { get; set; } = new();
        public List<EmailAddress> CcRecipients { get; set; } = new();
        public List<EmailAddress> BccRecipients { get; set; } = new();
        public List<AttachmentInfo> Attachments { get; set; } = new();
        public string ConversationId { get; set; } = "";
        public string ConversationTopic { get; set; } = "";
        public ImportanceLevel Importance { get; set; }
        public bool IsRead { get; set; }
    }

    public class EmailAddress
    {
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
    }

    public class AttachmentInfo
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public int Size { get; set; }
        public string ContentType { get; set; } = "";
    }

    public class UserProfile
    {
        public string DisplayName { get; set; } = "";
        public string EmailAddress { get; set; } = "";
        public string TimeZone { get; set; } = "";
        public string? UserId { get; internal set; }
        public string? UserName { get; internal set; }
        public string? Email { get; internal set; }
        public string? FirstName { get; internal set; }
        public string? LastName { get; internal set; }
        public List<string>? Roles { get; internal set; }
        public DateTime LastLogin { get; internal set; }
    }

    public enum ImportanceLevel
    {
        Low,
        Normal,
        High
    }
}
