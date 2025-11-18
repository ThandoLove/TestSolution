
namespace OutlookBlazorTestApp2.Data.Models
{
    public class MatchResult
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string EntityType { get; set; } = ""; // Contact, Lead, Opportunity
        public double Score { get; set; } = 0; // 0..1
        public string PrimaryEmail { get; set; } = "";
    }

    public class LinkRequest
    {
        public string EmailId { get; set; } = ""; // optional: message id
        public EmailContext EmailContext { get; set; } = new();
        public string TargetRecordId { get; set; } = "";
        public string TargetEntityType { get; set; } = "";
        public string UserId { get; set; } = "";
    }

    public class LinkResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
    }
}
