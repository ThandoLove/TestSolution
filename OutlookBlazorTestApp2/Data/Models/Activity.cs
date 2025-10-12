using OutlookBlazorTestApp2.Data.Models.Supporting;

namespace OutlookBlazorTestApp2.Data.Models
{
    public class Activity
    {
        public Guid Id { get; set; }   // ✅ use Guid like Contact for consistency

        public ActivityType Type { get; set; }   // enum (e.g., Call, Email, Meeting)
        public string Subject { get; set; } = "";
        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public PriorityLevel Priority { get; set; }

        public List<EntityReference>? RelatedEntities { get; set; } = new();



        public string? Location { get; set; }

        public ActivityStatus Status { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        // Optional: relations
        public Guid? ContactId { get; set; }
        public Contact? Contact { get; set; }
    }

    
}
