
namespace OutlookBlazorTestApp2.Data.Models
{
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

    public class EmailSender
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }

    public class EmailRecipient
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }

    public class EmailAttachment
    {
        public string FileName { get; set; } = string.Empty;
        public long Size { get; set; } // bytes
        public string ContentType { get; set; } = string.Empty;
        // Add other fields as needed, e.g., byte[] Content
    }


    public enum ImportanceLevel
    {
        Low,
        Normal,
        High
    }
}
